# codegen/entity_generator.py

import re
import os
from collections import defaultdict
from codegen.util import safe_pascal_case, to_pascal_case # 유틸리티 함수 import
# from codegen.file_utils import write_file # 파일 쓰기는 내부 함수 사용 또는 file_utils 사용 가능

# --- 타입 매핑 함수 ---
# (erd_parser나 다른 곳에서 공유될 수 있지만, 여기서도 정의해 둡니다)
def map_type(ftype, enums, nullable=False):
    """DB 타입을 C# 타입으로 매핑"""
    TYPE_MAP = {
        "uuid": "Guid", "guid": "Guid", "Guid": "Guid",
        "string": "string", "text": "string", # text 추가
        "int": "int", "integer": "int", "long": "long", # long 추가
        "bool": "bool", "boolean": "bool",
        "datetime": "DateTime", "timestamp": "DateTime",
        "float": "float", "real": "float",
        "double": "double", "numeric": "decimal", # numeric -> decimal 추가
        "json": "string", "jsonb": "string" # json/jsonb은 string 또는 별도 타입 매핑
    }
    # Enum 타입 처리
    if ftype in enums:
        # Enum 이름 자체를 타입으로 사용 (PascalCase 변환은 C# 코드 생성 시점에)
        return ftype # 원본 Enum 이름 반환

    ftype_norm = str(ftype).lower()
    # 기본 타입 매핑
    cstype = TYPE_MAP.get(ftype_norm, "string") # 매핑 없으면 string

    # Nullable 처리: C# 값 타입(struct)만 nullable(?) 추가, 참조 타입(string 등)은 제외
    value_types = ["Guid", "int", "long", "bool", "DateTime", "float", "double", "decimal"] # C# 값 타입 목록
    if nullable and cstype in value_types:
        return f"{cstype}?"
    else:
        return cstype

# --- 폴더/네임스페이스 결정 함수 ---
def get_folder_and_namespace(tname_pascal, layer, is_join_entity=False):
    """엔티티 이름과 레이어를 기반으로 폴더 경로와 네임스페이스 결정"""
    # 엔티티 카테고리 분류 (기존 로직 사용)
    def classify_entity_category(tname, is_join):
        if is_join: return "Refs"
        if tname.endswith("Log"): return "Logs"
        if tname.endswith("Detail") or tname.endswith("Requirement") or \
           tname.endswith("Probability") or tname.endswith("AvailableTime"): return "Details"
        if tname.endswith("Definition") or tname.endswith("Product") or \
           tname.endswith("Effect") or tname.endswith("Rune") or \
           tname.endswith("Enhancement") or tname.endswith("Option"): return "Definitions"
        if "Allowed" in tname or "Ref" in tname or "Map" in tname or \
           "Link" in tname or tname.endswith("Mapping") or \
           tname.endswith("Prerequisite"): return "Refs" # Mapping, Prerequisite 추가
        if tname.startswith("Player") or tname.endswith("Instance") or \
           tname.endswith("Progress") or tname.endswith("Achievement") or \
           tname.endswith("Quest") or tname.endswith("History") or \
           tname.endswith("Point"): return "Progress" # History, Point 추가
        # UserContent 관련 분류 추가
        if tname.startswith("User") or tname.endswith("Like") or \
           tname.endswith("Comment") or tname.endswith("Favorite") or \
           tname.endswith("Board") or tname.endswith("Post"): return "UserContents"
        # 추가적인 분류 규칙 (예: Config, Setting 등)
        if tname.endswith("Config") or tname.endswith("Setting"): return "Configs"
        # 기본값
        return "Entities" # General 대신 Entities 사용

    category = classify_entity_category(tname_pascal, is_join_entity)
    category_pascal = to_pascal_case(category) # 카테고리 이름도 PascalCase

    if layer == "Entities": # Infrastructure Entities
        folder = os.path.join("Infrastructure", "Entities", category_pascal)
        ns = f"Server.Infrastructure.Entities.{category_pascal}"
    elif layer == "Domain": # Core Domain Entities
        folder = os.path.join("Core", "Entities", category_pascal)
        ns = f"Server.Core.Entities.{category_pascal}"
    elif layer == "DTOs": # Application DTOs
        folder = os.path.join("Application", "DTOs", category_pascal)
        ns = f"Server.Application.DTOs.{category_pascal}"
    else:
        raise ValueError(f"Unknown layer: {layer}")

    return folder, ns


# --- 파일 쓰기 유틸리티 ---
def write_to_file(path, code):
    """주어진 경로에 C# 코드 쓰기 (디렉토리 자동 생성)"""
    try:
        norm_path = os.path.normpath(path)
        os.makedirs(os.path.dirname(norm_path), exist_ok=True)
        with open(norm_path, "w", encoding="utf-8") as f:
            f.write(code)
        # print(f"[WRITE] Generated file: {norm_path}") # 성공 로그 필요시 사용
    except Exception as e:
        print(f"[ERROR] Failed writing to file {path}: {e}")


# --- info_map 조회 헬퍼 ---
def info_map_lookup(target_entity_pascal, info_map):
    """PascalCase 이름으로 info_map에서 원본 테이블 정보 찾기"""
    if not info_map: return None
    # PascalCase 그대로 또는 Entity 접미사 제거/추가 등 시도
    candidates = [target_entity_pascal]
    if target_entity_pascal.endswith("Entity"):
         candidates.append(target_entity_pascal[:-6])
    else:
         candidates.append(target_entity_pascal + "Entity")
    # 원본 테이블 이름은 소문자/스네이크 케이스일 수 있으므로 추가 변환 불필요
    # info_map의 키가 PascalCase로 되어있다고 가정

    for cand in candidates:
        if cand in info_map:
            return info_map[cand]
    # print(f"[WARN] info_map_lookup: Could not find info for '{target_entity_pascal}'")
    return None


# --- 주 엔티티 생성 함수 ---
def generate_entity(table_name, info, navs, enums, folder, ns, info_map):
    """주어진 정보를 바탕으로 C# 엔티티 클래스 코드 생성"""
    self_ref_relations = [] # 자체 참조 관계 정보 저장용

    # --- 순수 조인 테이블 필터링 로직 제거 ---
    # (main.py에서 호출 전에 필터링하므로 여기서는 제거)

    entity_name_pascal = to_pascal_case(table_name)
    is_domain_layer = 'Core/Entities' in folder.replace('\\', '/').replace(os.sep, '/')
    # 클래스 이름 결정 (Domain 레이어는 Entity 접미사 없음)
    class_name = entity_name_pascal if is_domain_layer else f"{entity_name_pascal}Entity"
    class_decl = f"public class {class_name}"

    # using 문 관리
    usings = set()
    # Infrastructure 레이어 엔티티는 어트리뷰트 사용
    if not is_domain_layer:
        usings.add("using System.ComponentModel.DataAnnotations;")
        usings.add("using System.ComponentModel.DataAnnotations.Schema;")
        # 기본값 어트리뷰트 위해 추가 (선택적)
        # usings.add("using System.ComponentModel;")

    # Enum 사용 시 Server.Enums 추가
    if any(field["type"] in enums for field in info["fields"]):
        usings.add("using Server.Enums;")

    # Navigation 속성 위한 using 추가
    nav_namespaces = set()
    for nav in navs.get(entity_name_pascal, []):
        target_pascal = nav["target"] # 네비게이션 대상 엔티티 이름 (PascalCase)
        # 대상 엔티티의 네임스페이스 찾기 (Domain/Infra 동일한 분류 사용)
        target_folder, target_ns = get_folder_and_namespace(target_pascal, "Domain" if is_domain_layer else "Entities")
        if target_ns != ns: # 현재 엔티티와 다른 네임스페이스면 using 추가
            nav_namespaces.add(f"using {target_ns};")
    usings.update(nav_namespaces)

    # 코드 라인 생성 시작
    lines = []
    lines.extend(sorted(list(usings))) # 정렬된 using 문 추가
    lines.append("")
    lines.append(f"namespace {ns}")
    lines.append("{")

    # 테이블 이름 어트리뷰트 (Infra 레이어만)
    if not is_domain_layer:
        lines.append(f"    [Table(\"{table_name}\")]") # 원본 테이블 이름 사용

    lines.append(f"    {class_decl}")
    lines.append("    {")

    # 복합 키 정보 주석 (Infra 레이어만, 참고용)
    pk_fields_orig = info.get("pk_fields", [])
    if not is_domain_layer and len(pk_fields_orig) > 1:
        pk_fields_str = ", ".join(pk_fields_orig)
        lines.append(f"        // 복합키: [{pk_fields_str}] -> OnModelCreating에서 HasKey 설정 필요")

    # 필드 속성 생성
    generated_fk_props = set() # 자동 생성된 FK 속성 추적 (중복 방지)
    for field in info["fields"]:
        prop_name_pascal = safe_pascal_case(field["name"]) # C# 작명 규칙 적용
        is_pk = field["name"] in pk_fields_orig

        # Infrastructure 레이어 어트리뷰트 추가
        if not is_domain_layer:
            # Column 이름 명시
            lines.append(f"        [Column(\"{field['name']}\")]") # DB 컬럼명은 원본 이름 사용

            # Key 어트리뷰트 (단일 PK인 경우)
            if is_pk and len(pk_fields_orig) == 1:
                 lines.append("        [Key]")
                 # Guid PK 자동 생성 방지 (DB에서 생성하도록 유도 - context_generator에서 설정)
                 # if map_type(field["type"], enums).lower() == "guid":
                 #      lines.append("        [DatabaseGenerated(DatabaseGeneratedOption.None)]")

            # 기타 어트리뷰트 (Required, MaxLength 등)
            attributes = field.get("attributes", [])
            for attr in attributes:
                attr_lower = attr.lower()
                if attr_lower == "required":
                    # Nullable 아닌 값타입은 Required 불필요, 참조타입(string)만 추가
                    cstype = map_type(field["type"], enums, field.get("nullable", False))
                    if cstype == "string": # 또는 다른 참조 타입
                         lines.append("        [Required]")
                elif attr_lower.startswith("maxlength"): # 예: MaxLength(100)
                    lines.append(f"        [{attr}]") # 원본 그대로 사용
                # 다른 EF Core 어트리뷰트 필요시 추가 (예: DefaultValue, ConcurrencyCheck 등)
                # elif attr_lower.startswith("default"): ...
                # elif attr_lower == "concurrencycheck": lines.append("        [ConcurrencyCheck]")

        # C# 타입 결정
        csharp_type = map_type(field["type"], enums, field.get("nullable", False))
        # Enum 타입이면 PascalCase로 변환
        if field["type"] in enums:
            csharp_type = to_pascal_case(csharp_type)

        # 속성 정의 추가
        lines.append(f"        public {csharp_type} {prop_name_pascal} {{ get; set; }}")
        # FK 속성 추적용
        if 'ref' in [a.lower() for a in field.get('attributes', [])]:
             generated_fk_props.add(prop_name_pascal)


    # 네비게이션 속성 생성
    lines.append("") # 구분선
    lines.append("        #region Navigation Properties") # 네비게이션 영역 표시

    processed_nav_targets = set() # 네비게이션 중복 처리 방지 (대상 엔티티 기준)
    for nav in navs.get(entity_name_pascal, []):
        target_entity_pascal = nav["target"] # 대상 엔티티 이름 (PascalCase)
        nav_prop_name = safe_pascal_case(nav["prop"]) # 네비게이션 속성 이름
        nav_type = nav["type"] # 'single' or 'collection'
        label = nav["label"] # 원본 관계 레이블 (주석용)
        is_self_ref = (target_entity_pascal == entity_name_pascal)

        # 네비게이션 속성 이름 조정 (자기 참조 등)
        if is_self_ref:
             nav_prop_name = "Parent" if nav_type == "single" else "Children"
        elif not nav_prop_name: # 속성 이름 없으면 대상 엔티티 이름 사용
             nav_prop_name = target_entity_pascal if nav_type == 'single' else pluralize(target_entity_pascal)
             nav_prop_name = safe_pascal_case(nav_prop_name) # 안전한 이름 변환


        # 네비게이션 대상 키 (중복 생성 방지)
        nav_key = (nav_type, target_entity_pascal, nav_prop_name)
        if nav_key in processed_nav_targets: continue
        processed_nav_targets.add(nav_key)

        # 대상 엔티티 클래스 이름 결정 (Domain/Infra 레이어 맞춤)
        target_class_name = target_entity_pascal if is_domain_layer else f"{target_entity_pascal}Entity"

        # --- 외래 키(FK) 속성 자동 생성 (단일 네비게이션 && Domain 레이어 아닐 때) ---
        if nav_type == "single" and not is_domain_layer:
            # 예상 FK 속성 이름 (네비게이션 속성 이름 + "Id")
            # 자기 참조면 ParentId, 아니면 TargetEntityId (예: AccountId)
            expected_fk_prop_name = "ParentId" if is_self_ref else f"{target_entity_pascal}Id"

            # 해당 FK 속성이 이미 필드 목록에 정의되지 않았는지 확인
            if expected_fk_prop_name not in generated_fk_props:
                # 대상(Principal) 엔티티의 PK 타입 추론
                pk_type = "Guid" # 기본값
                target_info = info_map_lookup(target_entity_pascal, info_map)
                if target_info and target_info.get("pk_fields"):
                    pk_field_name_orig = target_info["pk_fields"][0]
                    pk_field_info = next((f for f in target_info["fields"] if f["name"] == pk_field_name_orig), None)
                    if pk_field_info:
                         # PK 필드 정보로 타입 결정 (nullable=False 가정, FK는 nullable일 수 있음 - 이건 nav nullability로 결정해야 함)
                         # TODO: FK의 Nullable 여부 결정 로직 필요 (ERD 정보나 네비게이션 속성 optionality 기반)
                         pk_type = map_type(pk_field_info["type"], enums, nullable=False) # 기본적으로 non-nullable

                # FK 속성 생성 (Nullable 여부 결정 필요)
                # TODO: 관계의 Optionality를 파악하여 FK Nullable 결정 (예: nullable=True)
                fk_csharp_type = pk_type # 우선 non-nullable로 생성
                lines.append(f"        public {fk_csharp_type} {expected_fk_prop_name} {{ get; set; }} // Auto-generated FK")
                generated_fk_props.add(expected_fk_prop_name) # 생성된 FK 추적

        # --- 네비게이션 속성 생성 ---
        if label: # 관계 레이블 주석 추가
             lines.append(f"        /// <summary> Relation Label: {label} </summary>")

        if nav_type == "collection":
             # 컬렉션 초기화 추가 (HashSet 사용)
             lines.append(f"        public virtual ICollection<{target_class_name}> {nav_prop_name} {{ get; set; }} = new HashSet<{target_class_name}>();")
        else: # 'single'
             # virtual 키워드 추가 (Lazy Loading 지원 위함)
             lines.append(f"        public virtual {target_class_name} {nav_prop_name} {{ get; set; }}") # 기본적으로 Nullable 허용


        # 자기 참조 관계 정보 저장 (context_generator에서 사용)
        if is_self_ref:
             # 한 번만 추가하도록 조정 필요 (parent/children 쌍으로)
             parent_prop = "Parent"
             children_prop = "Children"
             parent_id_prop = "ParentId"
             # 이미 존재하는지 확인 후 추가
             if not any(sr['entity'] == entity_name_pascal and sr['parent_prop'] == parent_prop for sr in self_ref_relations):
                 self_ref_relations.append({
                     "entity": entity_name_pascal, # 현재 엔티티
                     "parent_prop": parent_prop,
                     "children_prop": children_prop,
                     "parent_id_prop": parent_id_prop
                 })


    lines.append("        #endregion") # 네비게이션 영역 끝
    lines.append("    }") # 클래스 닫기
    lines.append("}") # 네임스페이스 닫기

    # 최종 C# 코드
    code = "\n".join(lines)

    # 파일 경로 및 쓰기
    out_path = os.path.normpath(os.path.join(folder, f"{class_name}.cs"))

    write_to_file(out_path, code) # write_to_file 함수 사용

    return code, self_ref_relations


# --- 명시적 조인 엔티티 생성 함수 삭제 ---
# def generate_nm_join_entity(...): <--- 이 함수 전체 삭제