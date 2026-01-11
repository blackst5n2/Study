"""
WIW DDD 자동화 전체 파이프라인 엔트리포인트
"""
import logging
import os
import re # 정규식 사용 위해 추가
from collections import defaultdict

# codegen 패키지 내 모듈 import
from codegen.erd_parser import parse_erd, map_type # map_type 추가
from codegen.entity_generator import get_folder_and_namespace, generate_entity
# generate_nm_join_entity는 제거되었으므로 import 불필요
from codegen.automapper_generator import generate_automapper_profile
from codegen.valueobject_generator import generate_value_object # 필요시 사용
from codegen.dto_generator import generate_dto
from codegen.repository_generator import generate_repository_interface, generate_repository_impl
from codegen.service_generator import generate_application_service
from codegen.context_generator import generate_appdbcontext
from codegen.pluralize_util import pluralize
from codegen.di_generator import generate_dependency_injection
from codegen.enum_generator import generate_enums
from codegen.util import normalize_reltype, to_pascal_case, safe_pascal_case # 유틸 함수 import

# --- 테이블 분류 헬퍼 함수 ---
def classify_table(table_name, info):
    """테이블 정보를 바탕으로 종류를 분류 ('pure_join', 'payload_join', 'regular')"""
    fields = info.get('fields', [])
    field_count = len(fields)

    # 모든 필드가 pk이자 ref인지 확인
    is_all_pk_ref = field_count >= 2 and all(
        'pk' in [a.lower() for a in f.get('attributes',[])] and \
        'ref' in [a.lower() for a in f.get('attributes',[])]
        for f in fields
    )

    # 순수 조인 테이블 조건: 정확히 필드 2개, 모두 pk+ref, 페이로드 없음
    if field_count == 2 and is_all_pk_ref:
        # 페이로드 확인: 필수 키 외 다른 키가 필드에 있는지 확인 (attributes_meta 추가)
        has_payload = any(
            len(set(f.keys()) - {'name', 'type', 'attributes', 'key', 'ref', 'nullable', 'attributes_meta'}) > 0
            for f in fields
        )
        if not has_payload:
            return 'pure_join'

    # 페이로드 조인 테이블 조건 (여기서는 별도 처리 안 함, 필요시 로직 추가)
    # if is_all_pk_ref:
    #     return 'payload_join'

    return 'regular'

# --- 네비게이션 빌드 함수 (수정된 버전) ---
def build_navigation(tables, relations, table_classification, pure_join_table_names):
    """
    관계 정보를 바탕으로 네비게이션 속성 정보를 생성합니다.
    순수 조인 테이블을 사용한 암시적 다대다 관계를 올바르게 처리합니다.

    Args:
        tables (dict): 테이블 이름과 필드 정보가 담긴 딕셔너리.
        relations (list): 관계 정보가 담긴 딕셔너리 리스트.
        table_classification (dict): 테이블 이름별 분류 ('pure_join', 'regular' 등).
        pure_join_table_names (set): 순수 조인 테이블의 원본 이름 Set.
    """
    navs = defaultdict(list)
    nav_keys = set() # 중복 네비게이션 속성 추가 방지
    logging.debug(f"Starting build_navigation. Pure join tables: {pure_join_table_names}")

    # 순수 조인 테이블이 연결하는 엔티티 정보 저장용
    # 형식: { join_table_name: [ (principal_entity_pascal, fk_field_in_join_table), ... ] }
    join_table_links = defaultdict(list)

    # --- 1단계: 순수 조인 테이블 링크 식별 및 일반 관계 처리 ---
    for i, rel in enumerate(relations):
        left_orig = rel["left"]
        right_orig = rel["right"]
        left_pascal = to_pascal_case(left_orig)
        right_pascal = to_pascal_case(right_orig)
        reltype_original = rel["rel"]
        label = rel["label"]

        left_type = table_classification.get(left_orig, 'regular')
        right_type = table_classification.get(right_orig, 'regular')

        # --- A. 순수 조인 테이블 관련 관계 식별 ---
        # A1: JoinTable }o--|| Principal (JoinTable이 Dependent)
        if left_orig in pure_join_table_names and right_type != 'pure_join':
            join_table_name = left_orig
            principal_name = right_pascal
            # 조인 테이블 내에서 Principal을 참조하는 FK 필드 이름 추출 (erd_parser가 제공해야 함)
            fk_field_name = rel.get("fk_field")
            join_table_links[join_table_name].append( (principal_name, fk_field_name) )
            logging.debug(f"[NAV BUILDER] Relation {i}: Found link from PURE join table {join_table_name} to Principal {principal_name} (FK in join: {fk_field_name})")
            continue # 조인 테이블 자체로의 네비게이션은 생성 안 함

        # A2: Principal ||--o{ JoinTable (JoinTable이 Dependent)
        elif right_orig in pure_join_table_names and left_type != 'pure_join':
            join_table_name = right_orig
            principal_name = left_pascal
            # 조인 테이블 내에서 Principal을 참조하는 FK 필드 이름 추출 (erd_parser가 제공해야 함)
            fk_field_name = rel.get("fk_field")
            join_table_links[join_table_name].append( (principal_name, fk_field_name) )
            logging.debug(f"[NAV BUILDER] Relation {i}: Found link from Principal {principal_name} to PURE join table {join_table_name} (FK in join: {fk_field_name})")
            continue # 조인 테이블 자체로의 네비게이션은 생성 안 함

        # A3: 순수 조인 테이블 간의 관계 (일반적이지 않음, 건너뛰기)
        elif left_orig in pure_join_table_names and right_orig in pure_join_table_names:
             logging.warning(f"[NAV BUILDER] Relation {i}: Skipping relationship between two pure join tables: {left_orig} <-> {right_orig}")
             continue

        # --- B. 일반 관계 처리 (순수 조인 테이블 미포함) ---
        normalized_type = normalize_reltype(reltype_original)

        # B1: 자체 참조 (일반 엔티티)
        if left_pascal == right_pascal:
             entity_name = left_pascal
             # 'Parent' (single) 추가
             parent_prop_name = "Parent"
             nav_key_parent = (entity_name, parent_prop_name)
             if nav_key_parent not in nav_keys:
                 navs[entity_name].append({"type": "single", "target": entity_name, "prop": parent_prop_name, "label": label})
                 nav_keys.add(nav_key_parent)
             # 'Children' (collection) 추가
             children_prop_name = "Children"
             nav_key_children = (entity_name, children_prop_name)
             if nav_key_children not in nav_keys:
                  navs[entity_name].append({"type": "collection", "target": entity_name, "prop": children_prop_name, "label": label})
                  nav_keys.add(nav_key_children)
             continue # 아래 로직 건너뛰기

        # B2: 1:N 관계 (일반 엔티티 간)
        elif normalized_type == "||--o{":
            principal_class, dependent_class = (right_pascal, left_pascal) if reltype_original.strip().endswith('||') else \
                                             (left_pascal, right_pascal) if reltype_original.strip().startswith('||') else \
                                             (None, None)
            if principal_class and dependent_class:
                # Principal에 Collection 추가
                prop_collection = pluralize(dependent_class)
                nav_key_principal = (principal_class, prop_collection)
                if nav_key_principal not in nav_keys:
                    navs[principal_class].append({"type": "collection", "target": dependent_class, "prop": prop_collection, "label": label})
                    nav_keys.add(nav_key_principal)
                # Dependent에 Single 추가
                prop_single = principal_class
                nav_key_dependent = (dependent_class, prop_single)
                if nav_key_dependent not in nav_keys:
                    navs[dependent_class].append({"type": "single", "target": principal_class, "prop": prop_single, "label": label})
                    nav_keys.add(nav_key_dependent)

        # B3: 1:1 관계 (일반 엔티티 간)
        elif normalized_type == "||--||":
             nav_key_left = (left_pascal, right_pascal)
             nav_key_right = (right_pascal, left_pascal)
             if nav_key_left not in nav_keys:
                 navs[left_pascal].append({"type": "single", "target": right_pascal, "prop": right_pascal, "label": label})
                 nav_keys.add(nav_key_left)
             if nav_key_right not in nav_keys:
                 navs[right_pascal].append({"type": "single", "target": left_pascal, "prop": left_pascal, "label": label})
                 nav_keys.add(nav_key_right)

        # B4: 일반 엔티티 간 직접 N:M (ERD 오류 또는 명시적 조인 엔티티 필요 가능성)
        elif normalized_type in ("o{--o{", "o|--o{"):
             logging.warning(f"[NAV BUILDER] Detected direct N:M between non-join tables: {left_pascal} <-> {right_pascal}. Ensure this is intended or use an explicit join table.")
             # 기본적으로 양쪽에 컬렉션 추가 (암시적 처리 시도)
             nav_key_left = (left_pascal, pluralize(right_pascal))
             nav_key_right = (right_pascal, pluralize(left_pascal))
             if nav_key_left not in nav_keys:
                 navs[left_pascal].append({"type": "collection", "target": right_pascal, "prop": pluralize(right_pascal), "label": label})
                 nav_keys.add(nav_key_left)
             if nav_key_right not in nav_keys:
                 navs[right_pascal].append({"type": "collection", "target": left_pascal, "prop": pluralize(left_pascal), "label": label})
                 nav_keys.add(nav_key_right)

        # TODO: 다른 일반 관계 타입 처리 추가


    # --- 2단계: 수집된 순수 조인 테이블 링크 기반으로 N:M 네비게이션 속성 생성 ---
    print(f"[NAV BUILDER] Processing {len(join_table_links)} potential N:M links via pure join tables...")
    processed_nm_pairs = set() # 처리된 N:M 쌍 (중복 방지)

    for join_table, principals_info in join_table_links.items():
        # 순수 조인 테이블은 정확히 두 개의 Principal 엔티티를 연결해야 함
        if len(principals_info) == 2:
            (principal_A, fk_A_in_join) = principals_info[0]
            (principal_B, fk_B_in_join) = principals_info[1]

            # 중복 처리 방지 키 (정렬된 엔티티 이름 쌍)
            pair_key = tuple(sorted((principal_A, principal_B)))
            if pair_key in processed_nm_pairs:
                 continue
            processed_nm_pairs.add(pair_key)

            logging.debug(f"[NAV BUILDER] Found N:M link: {principal_A} <-> {principal_B} via {join_table}")

            # N:M 자기 참조 처리 (예: QuestPrerequisite)
            if principal_A == principal_B:
                 entity_name = principal_A
                 # 조인 테이블의 FK 이름으로 속성 이름 구분 시도
                 # 예: quest_id -> Quests, prerequisite_quest_id -> PrerequisiteQuests
                 prop1_name = f"Related{pluralize(entity_name)}1" # 기본 이름 1
                 prop2_name = f"Related{pluralize(entity_name)}2" # 기본 이름 2
                 try:
                    # fk_A_in_join, fk_B_in_join 이름 분석하여 더 나은 이름 결정 (예시)
                    name1 = pluralize(safe_pascal_case(fk_A_in_join.replace('_id', ''))) if fk_A_in_join else prop1_name
                    name2 = pluralize(safe_pascal_case(fk_B_in_join.replace('_id', ''))) if fk_B_in_join else prop2_name
                    # 'prerequisite' 같은 키워드로 역할 구분 시도
                    if 'prerequisite' in name1.lower(): prop1_name, prop2_name = name1, name2
                    elif 'prerequisite' in name2.lower(): prop1_name, prop2_name = name2, name1
                    else: prop1_name, prop2_name = name1, name2 # 이름 충돌 시 기본값 사용
                 except Exception: pass # 이름 결정 실패 시 기본값 사용

                 # 양방향 컬렉션 네비게이션 정보 추가
                 nav_key1 = (entity_name, prop1_name)
                 if nav_key1 not in nav_keys:
                     navs[entity_name].append({"type": "collection", "target": entity_name, "prop": prop1_name, "label": f"N:M Self via {join_table} ({fk_A_in_join})"})
                     nav_keys.add(nav_key1)
                 nav_key2 = (entity_name, prop2_name)
                 if nav_key2 not in nav_keys:
                      navs[entity_name].append({"type": "collection", "target": entity_name, "prop": prop2_name, "label": f"N:M Self via {join_table} ({fk_B_in_join})"})
                      nav_keys.add(nav_key2)

            # 일반적인 N:M 관계 처리
            else:
                # Principal A에 Collection<Principal B> 추가
                prop_B_on_A = pluralize(principal_B)
                nav_key_A = (principal_A, prop_B_on_A)
                if nav_key_A not in nav_keys:
                    navs[principal_A].append({"type": "collection", "target": principal_B, "prop": prop_B_on_A, "label": f"N:M via {join_table}"})
                    nav_keys.add(nav_key_A)

                # Principal B에 Collection<Principal A> 추가
                prop_A_on_B = pluralize(principal_A)
                nav_key_B = (principal_B, prop_A_on_B)
                if nav_key_B not in nav_keys:
                     navs[principal_B].append({"type": "collection", "target": principal_A, "prop": prop_A_on_B, "label": f"N:M via {join_table}"})
                     nav_keys.add(nav_key_B)
        else:
            # 조인 테이블이 2개가 아닌 다른 수의 엔티티를 연결하는 경우 (보통 비정상)
            logging.warning(f"[NAV BUILDER] Join table {join_table} links {len(principals_info)} principals ({[p[0] for p in principals_info]}) instead of 2. Skipping N:M nav property generation.")


    # 최종 navs 내용 확인용 로그 (필요시 주석 해제)
    # for entity, props in navs.items():
    #      print(f"  Navs for {entity}: {props}")
    return navs

# --- 메인 파이프라인 함수 ---
def run_pipeline(erd_path):
    """전체 코드 생성 파이프라인 실행"""
    tables, enums, relations = parse_erd(erd_path)

    # --- 1. 테이블 분류 ---
    table_classification = {}
    pure_join_table_names = set()
    # payload_join_table_names = set() # 필요시 사용
    regular_entity_table_names = set()

    for table_name, info in tables.items():
        classification = classify_table(table_name, info)
        table_classification[table_name] = classification
        if classification == 'pure_join':
            pure_join_table_names.add(table_name)
        # elif classification == 'payload_join':
        #     payload_join_table_names.add(table_name)
        else:
            regular_entity_table_names.add(table_name)

    # --- 2. Enum 생성 ---
    enum_output_dir = "Enums" # 출력 경로 지정
    generate_enums(enums, output_dir=enum_output_dir)

    # --- 3. 네비게이션 정보 빌드 ---
    navs = build_navigation(tables, relations, table_classification, pure_join_table_names)
    print("[LOG] Built navigation information.")

    # --- 4. Entity / DTO 생성 (순수 조인 테이블 제외) ---
    entities_pascal = [] # 생성된 엔티티 목록 (PascalCase)
    generated_pascal_names = set()
    info_map = {to_pascal_case(table): info for table, info in tables.items()}
    globals()["info_map"] = info_map # entity_generator에서 사용될 수 있음

    all_self_ref_relations = [] # 자체 참조 관계 누적용

    print("[LOG] Starting Entity and DTO generation...")
    generated_entity_count = 0
    generated_dto_count = 0
    for table_name, info in tables.items():
        if table_classification[table_name] == 'pure_join':
            # print(f"[SKIP GEN] Skipping entity/DTO for pure join table: {table_name}")
            continue

        entity_name_pascal = to_pascal_case(table_name)
        if entity_name_pascal in generated_pascal_names: continue # 중복 방지
        if not info.get("pk_fields"): continue # PK 없으면 건너뛰기

        # is_payload_join = table_classification[table_name] == 'payload_join' # 필요시 사용

        # 엔티티 생성 시도
        try:
            # Infrastructure Entity
            db_folder, db_ns = get_folder_and_namespace(entity_name_pascal, "Entities", is_join_entity=False) # is_join_entity 필요시 수정
            code_infra, self_ref_rel1 = generate_entity(table_name, info, navs, enums, db_folder, db_ns, info_map)

            if code_infra: # 생성 성공 시
                all_self_ref_relations.extend(self_ref_rel1)

                # Domain Entity
                domain_folder, domain_ns = get_folder_and_namespace(entity_name_pascal, "Domain", is_join_entity=False) # is_join_entity 필요시 수정
                code_domain, self_ref_rel2 = generate_entity(table_name, info, navs, enums, domain_folder, domain_ns, info_map)
                all_self_ref_relations.extend(self_ref_rel2)

                entities_pascal.append(entity_name_pascal) # 성공 목록에 추가
                generated_entity_count += 1

                # DTO 생성
                dto_folder, dto_ns = get_folder_and_namespace(entity_name_pascal, "DTOs", is_join_entity=False)
                fields_for_dto = [
                    {
                        **f, "name": to_pascal_case(f["name"]),
                        "type": map_type(f["type"], enums, f.get("nullable", False)),
                    } for f in info["fields"]
                ]
                generate_dto(entity_name_pascal, fields_for_dto, dto_folder, dto_ns, enums=enums)
                generated_dto_count += 1

                generated_pascal_names.add(entity_name_pascal)
        except Exception as e:
            print(f"[ERROR] Failed generating entity/DTO for {table_name}: {e}")

    print(f"[LOG] Generated {generated_entity_count} Entities and {generated_dto_count} DTOs.")


    # --- 5. Aggregate Root 식별 ---
    aggregate_roots = []
    child_entities_pascal = set()

    for rel in relations:
        left_orig = rel["left"]
        right_orig = rel["right"]
        # 관계의 양쪽이 모두 정규 엔티티일 경우만 고려
        if left_orig in regular_entity_table_names and right_orig in regular_entity_table_names:
            rel_type_normalized = normalize_reltype(rel["rel"])
            # 1:N 관계에서 Dependent 식별 (Principal ||--o{ Dependent)
            if rel_type_normalized == "||--o{":
                if rel["rel"].strip().startswith('||'): # Left가 Principal
                    child_entities_pascal.add(to_pascal_case(right_orig))
                elif rel["rel"].strip().endswith('||'): # Right가 Principal
                     child_entities_pascal.add(to_pascal_case(left_orig))
            # TODO: 다른 소유 관계(예: 1:1에서 FK 가진 쪽)에 대한 자식 식별 로직 추가

    # Aggregate Root 후보 선정 로직
    AGGREGATE_ROOT_SUFFIXES = ["Log", "History", "Detail", "Mapping", "Record", "Stat"] # 예외 접미사
    def is_probably_not_root(entity_name): return any(entity_name.endswith(suffix) for suffix in AGGREGATE_ROOT_SUFFIXES)

    for table_name in regular_entity_table_names:
        entity_name_pascal = to_pascal_case(table_name)
        if entity_name_pascal.endswith("Definition"): # Definition 우선
            if entity_name_pascal not in aggregate_roots: aggregate_roots.append(entity_name_pascal)
            continue
        # 자식이 아니고, 예외 접미사가 아니면 루트 후보
        if entity_name_pascal not in child_entities_pascal and not is_probably_not_root(entity_name_pascal):
            if entity_name_pascal not in aggregate_roots: aggregate_roots.append(entity_name_pascal)

    # 필수 포함 루트 추가 (정규 엔티티 목록에 있을 경우)
    MUST_INCLUDE = ["Player", "Account", "Container"] # 예시
    for entity_name_pascal in MUST_INCLUDE:
        corresponding_orig_name = next((t for t in regular_entity_table_names if to_pascal_case(t) == entity_name_pascal), None)
        if corresponding_orig_name and entity_name_pascal not in aggregate_roots:
             aggregate_roots.append(entity_name_pascal)

    # 중복 제거 및 정렬 (선택적)
    aggregate_roots = sorted(list(set(aggregate_roots)))


    # --- 6. Repository / Service 생성 (Aggregate Root 대상) ---
    print("[LOG] Starting Repository and Service generation...")
    repo_count = 0
    service_count = 0
    for entity_name_pascal in aggregate_roots:
        original_table_name = next((t for t in tables if to_pascal_case(t) == entity_name_pascal), None)
        if not original_table_name: continue
        info = tables[original_table_name]

        try:
            repo_if_folder = os.path.join("Core", "Repositories")
            repo_impl_folder = os.path.join("Infrastructure", "Repositories")
            service_folder = os.path.join("Application", "Services")

            # PK 타입 결정
            pk_name = info["pk_fields"][0] if info.get("pk_fields") else "Id" # 기본값 Id
            pk_field = next((f for f in info["fields"] if f["name"] == pk_name), None)
            pk_type = map_type(pk_field["type"], enums, False) if pk_field else "Guid" # 기본값 Guid

            # 네임스페이스 결정
            dom_folder, dom_ns = get_folder_and_namespace(entity_name_pascal, "Domain")
            db_folder, db_ns = get_folder_and_namespace(entity_name_pascal, "Entities")
            repo_ns = "Server.Core.Repositories" # 고정 네임스페이스 사용 가능
            infra_ns = "Server.Infrastructure.Repositories" # 고정 네임스페이스 사용 가능

            # 'Code' 컬럼 Unique 여부 확인
            code_unique = any(
                f["name"].lower() == "code" and f["type"].lower() == "string" and "unique" in [a.lower() for a in f.get("attributes", [])]
                for f in info["fields"]
            )

            # 생성 함수 호출
            generate_repository_interface(entity_name_pascal, pk_type, dom_ns, repo_if_folder, code_unique=code_unique)
            generate_repository_impl(entity_name_pascal, pk_type, dom_ns, db_ns, repo_impl_folder, code_unique=code_unique)
            repo_count +=1
            generate_application_service(entity_name_pascal, repo_ns, service_folder, code_unique=code_unique)
            service_count += 1
        except Exception as e:
             print(f"[ERROR] Failed generating repo/service for {entity_name_pascal}: {e}")



    # --- 7. 명시적 조인 엔티티 생성 로직 제거됨 ---
    print("[LOG] Skipping explicit join entity generation (removed logic).")


    # --- 8. AppDbContext 생성 ---
    print("[LOG] Starting AppDbContext generation...")
    context_folder = os.path.join("Infrastructure", "Contexts")
    context_ns = "Server.Infrastructure.Contexts"
    # DbSet 대상: Aggregate Roots (+ 필요시 페이로드 조인 테이블 추가)
    dbset_target_entities = aggregate_roots
    # entity_fields 맵핑 (원본 이름 키 사용)
    entity_fields_map = {name: info for name, info in tables.items()}

    # --- 8. AppDbContext 생성 ---
    print("[SIMPLE CHECK] *** 8. AppDbContext 생성 단계 진입 ***") # 단계 진입 확인
    context_folder = os.path.join("Infrastructure", "Contexts")
    context_ns = "Server.Infrastructure.Contexts"
    dbset_target_entities = aggregate_roots # Aggregate Root 목록 사용
    entity_fields_map = {name: tables[name] for name in tables}

    # --- 중요: DbSet 대상 목록이 비어있는지 확인 ---
    if not dbset_target_entities:
        print("[SIMPLE CHECK] !!! 경고: DbSet 생성 대상(Aggregate Roots) 목록이 비어있습니다. AppDbContext 내용이 없을 수 있습니다.")
    # --- 확인 끝 ---

    try:
        generate_appdbcontext(
            dbset_entities=list(set(dbset_target_entities)), # DbSet 생성 대상 (PascalCase)
            fluent_entities=list(tables.keys()),       # Fluent API 설정 대상 (원본 이름)
            output_dir=context_folder,
            ns=context_ns,
            relations=relations,
            entity_fields=entity_fields_map,
            enums=enums,
            self_ref_relations=all_self_ref_relations,
            pure_join_table_names=pure_join_table_names # 순수 조인 테이블 이름 전달
        )
        print("[SIMPLE CHECK] 호출 완료: generate_appdbcontext") # 함수 호출이 끝났는지 확인
    except Exception as e:
        print(f"[SIMPLE CHECK] !!! 오류 발생 (generate_appdbcontext 호출 중): {e}")
        import traceback
        traceback.print_exc() # 상세 오류 출력


    # --- 9. AutoMapper 생성 ---
    print("[LOG] Starting AutoMapper Profile generation...")
    automapper_output_path="Application/MapperProfile.cs"
    try:
        # entities_pascal: 실제로 엔티티 클래스가 생성된 목록 사용
        generate_automapper_profile(entities_pascal, output_path=automapper_output_path)
    except Exception as e:
        print(f"[ERROR] Failed generating AutoMapper Profile: {e}")


    # --- 10. DI 생성 ---
    print("[LOG] Starting Dependency Injection generation...")
    di_folder = os.path.join("Infrastructure")
    try:
        # Aggregate Root Repository만 등록
        generate_dependency_injection(aggregate_roots, di_folder)
    except Exception as e:
         print(f"[ERROR] Failed generating Dependency Injection: {e}")


    print("\nCode generation pipeline completed.")


# --- 스크립트 실행 지점 ---
if __name__ == "__main__":
    erd_file_path = "erd_full.mmd" # ERD 파일 경로 지정
    if not os.path.exists(erd_file_path):
        print(f"[ERROR] ERD file not found at: {erd_file_path}")
    else:
        print(f"--- Starting code generation from: {erd_file_path} ---")
        run_pipeline(erd_file_path)
        print(f"--- Code generation finished ---")