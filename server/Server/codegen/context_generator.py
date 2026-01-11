# codegen/context_generator.py

"""
AppDbContext 등 Context 코드 생성 모듈
"""
import os
import re # N:M 관계 처리 위해 추가
from codegen.util import normalize_reltype, safe_pascal_case, to_pascal_case # 유틸 함수 import
from codegen.file_utils import write_file # 파일 쓰기 유틸 import
from codegen.pluralize_util import pluralize # 복수형 변환 유틸 import
from codegen.entity_generator import map_type

# --- AppDbContext 생성 함수 ---
def generate_appdbcontext(dbset_entities, # DbSet 생성 대상 엔티티 이름 리스트 (PascalCase, main.py에서 필터링됨)
                          fluent_entities, # Fluent API 설정 대상 테이블 이름 리스트 (원본 이름)
                          output_dir, ns,
                          relations=None, entity_fields=None, enums=None,
                          self_ref_relations=None,
                          pure_join_table_names=None): # 순수 조인 테이블 원본 이름 Set 전달받음
    """AppDbContext 코드 생성 및 파일 저장 (C# class, EF Core용)"""

    pure_join_tables = pure_join_table_names or set() # 전달받은 set 사용, 없으면 빈 set

    # --- 1. DbSet<T> 생성 ---
    # main.py에서 이미 Aggregate Root (+ 필요시 Payload Join Table) 목록을 전달한다고 가정
    seen_dbsets = set()
    dbset_lines = []
    for entity_pascal in sorted(dbset_entities):
        entity_name = safe_pascal_case(entity_pascal) # 이름 일관성
        prop_name = pluralize(entity_name) # 복수형으로 DbSet 속성명 생성
        key = (entity_name, prop_name)

        # 중복 및 순수 조인 테이블 확인 (이중 안전장치)
        original_table_name_check = next((t for t in pure_join_tables if to_pascal_case(t) == entity_name), None)
        if original_table_name_check:
            print(f"[CONTEXT SKIP DbSet] Skipping DbSet for pure join table: {entity_name}")
            continue # 순수 조인 테이블이면 DbSet 생성 안 함

        if key not in seen_dbsets:
            dbset_lines.append(f"    public DbSet<{entity_name}Entity> {prop_name} {{ get; set; }}")
            seen_dbsets.add(key)
        # else:
        #     print(f"[CONTEXT WARN] Duplicate DbSet key detected: {key}") # 필요시 로그

    dbsets = "\n".join(dbset_lines)


    # --- 2. Fluent API 생성 준비 ---
    fluent_lines = []
    entity_fields_map = {name: info for name, info in (entity_fields or {}).items()} # 원본 이름 키 사용

    # --- 2.1. 기본 설정 (Enum, PK, Index, Default 등) ---
    print("[CONTEXT] Generating basic Fluent API configurations (Enum, PK, Index, Default)...")
    if entity_fields_map and enums:
        for original_table_name in fluent_entities: # 모든 테이블 대상
            # 순수 조인 테이블은 기본 설정 불필요 (테이블 정의가 없으므로)
            if original_table_name in pure_join_tables:
                 continue

            entity_class = f"{safe_pascal_case(original_table_name)}Entity"
            info = entity_fields_map.get(original_table_name, {})
            fields = info.get('fields', [])
            if not fields: continue # 필드 정보 없으면 건너뛰기

            # Enum -> string 변환
            for field in fields:
                if field["type"] in enums:
                    prop_name = safe_pascal_case(field['name']) # PascalCase 속성명
                    fluent_lines.append(f"        modelBuilder.Entity<{entity_class}>().Property(e => e.{prop_name}).HasConversion<string>();")

            # Guid PK 기본값 (uuid_generate_v7)
            # (단일 PK이고 이름이 'id'이고 타입이 Guid/uuid 일 때)
            pk_fields = info.get("pk_fields", [])
            if len(pk_fields) == 1 and pk_fields[0].lower() == 'id':
                 pk_field_info = next((f for f in fields if f['name'] == pk_fields[0]), None)
                 if pk_field_info and pk_field_info['type'].lower() in ['uuid', 'guid']:
                       fluent_lines.append(f"        modelBuilder.Entity<{entity_class}>().Property(e => e.Id).HasDefaultValueSql(\"uuid_generate_v7()\");")


            # 복합/고유 인덱스 (ERD의 indexes 정보 사용)
            indexes = info.get('indexes', [])
            for idx in indexes:
                cols = idx.get('columns', [])
                if not cols: continue
                # 람다식 생성: e => new { e.Prop1, e.Prop2 }
                lambda_cols = ", ".join([f"e.{safe_pascal_case(c)}" for c in cols])
                index_builder = f"modelBuilder.Entity<{entity_class}>().HasIndex(e => new {{ {lambda_cols} }})"
                if idx.get('unique'):
                    index_builder += ".IsUnique();"
                else:
                    index_builder += ";"
                fluent_lines.append(f"        {index_builder}")

            # 단일 Unique 제약 조건 (필드 attributes 사용)
            for field in fields:
                if 'unique' in [a.lower() for a in field.get('attributes', [])]:
                    # 복합 인덱스에서 이미 처리된 경우 제외 (선택적)
                    is_in_composite_unique = any(idx.get('unique') and field['name'] in idx.get('columns',[]) for idx in indexes if len(idx.get('columns',[])) > 1)
                    if not is_in_composite_unique:
                        prop_name = safe_pascal_case(field['name'])
                        fluent_lines.append(f"        modelBuilder.Entity<{entity_class}>().HasIndex(e => e.{prop_name}).IsUnique();")

            # Default 값 설정 (필드 attributes 사용)
            for field in fields:
                for attr in field.get('attributes', []):
                    attr_lower = attr.lower()
                    if attr_lower.startswith('default:'):
                        default_val_str = attr.split(":", 1)[1].strip()
                        prop_name = safe_pascal_case(field['name'])
                        # SQL 함수/값 (백틱 ` `) 처리
                        if default_val_str.startswith('`') and default_val_str.endswith('`'):
                            sql_val = default_val_str.strip('`')
                            fluent_lines.append(f"        modelBuilder.Entity<{entity_class}>().Property(e => e.{prop_name}).HasDefaultValueSql(\"{sql_val}\");")
                        else:
                            # C# 리터럴 값 처리 (Enum, string, bool, 숫자 등)
                            csharp_literal = default_val_str # 기본값으로 시작
                            original_field_type = field['type'] # ERD에 정의된 원본 타입

                            # --- *** 수정된 부분 시작 *** ---
                            if original_field_type in enums:
                                # 필드 타입이 Enum 목록에 있으면 Enum 타입 이름 추가
                                enum_type_name_pascal = to_pascal_case(original_field_type)
                                enum_member_name_pascal = to_pascal_case(default_val_str) # 값도 PascalCase
                                csharp_literal = f"{enum_type_name_pascal}.{enum_member_name_pascal}" # 예: MailStatus.Unread
                            else:
                                # Enum이 아닌 다른 타입 처리 (기존 로직)
                                field_cstype = map_type(original_field_type, enums, False)
                                if field_cstype == 'string':
                                    escaped_val = default_val_str.replace('"', '\\"') # 내부 따옴표 이스케이프
                                    csharp_literal = f'"{escaped_val}"' # 문자열은 큰따옴표로 감싸기
                                elif field_cstype == 'bool':
                                    csharp_literal = default_val_str.lower() # C# bool은 소문자 (true/false)
                                # TODO: 숫자형(int, long, float 등)의 경우 필요시 타입 변환 또는 형식 확인 추가 가능
                                # 현재는 문자열 그대로 사용
                            fluent_lines.append(f"        modelBuilder.Entity<{entity_class}>().Property(e => e.{prop_name}).HasDefaultValue({csharp_literal});")


    # --- 2.2. 관계 설정 (Fluent API) ---
    print("[CONTEXT] Generating relationship configurations (Fluent API)...")
    self_ref_entities = {sr["entity"] for sr in (self_ref_relations or [])} # PascalCase 이름 사용
    if relations:
        generated_relations = set() # (Dependent, Principal, FK Property) 튜플 저장
        generated_nm_pairs = set()  # (EntityA, EntityB) 튜플 저장 (N:M 중복 방지)

        for rel in relations:
            left_original = rel["left"]
            right_original = rel["right"]
            left_pascal = safe_pascal_case(left_original)
            right_pascal = safe_pascal_case(right_original)

            rel_type_original = rel["rel"]
            rel_type_normalized = normalize_reltype(rel_type_original)

            # 자체 참조 건너뛰기 (별도 처리)
            if left_pascal == right_pascal and left_pascal in self_ref_entities:
                continue

            # 순수 조인 테이블로 향하는 관계는 여기서 설정하지 않음
            if left_original in pure_join_tables or right_original in pure_join_tables:
                 continue

            principal_class = None
            dependent_class = None
            fk_field_pascal = None # FK 속성 이름 (PascalCase)

            # --- 정규화된 타입 기준으로 관계 설정 ---

            # 1:N (Principal ||--o{ Dependent)
            if rel_type_normalized == "||--o{":
                # 역할 식별
                if rel_type_original.strip().endswith('||'):
                    principal_class = right_pascal
                    dependent_class = left_pascal
                elif rel_type_original.strip().startswith('||'):
                    principal_class = left_pascal
                    dependent_class = right_pascal
                else: continue # 역할 불분명

                if principal_class and dependent_class:
                    # FK 이름 결정 (Dependent에 존재)
                    fk_field_name = rel.get("fk_field") or f"{principal_class}Id" # Parser가 제공하거나 규칙 사용
                    fk_field_pascal = safe_pascal_case(fk_field_name)

                    key = (dependent_class, principal_class, fk_field_pascal)
                    if key in generated_relations: continue
                    generated_relations.add(key)

                    # 네비게이션 속성 이름 결정 (규칙 기반)
                    collection_nav_prop = pluralize(dependent_class) # Principal의 컬렉션 속성
                    single_nav_prop = principal_class           # Dependent의 단일 참조 속성

                    fluent_lines.append(f"        // 1:N Relationship: {principal_class} -> {dependent_class}")
                    fluent_lines.append(f"        modelBuilder.Entity<{dependent_class}Entity>()")
                    fluent_lines.append(f"            .HasOne(e => e.{single_nav_prop})")
                    fluent_lines.append(f"            .WithMany(e => e.{collection_nav_prop})")
                    fluent_lines.append(f"            .HasForeignKey(e => e.{fk_field_pascal});")

            # 1:1 관계
            elif rel_type_normalized == "||--||":
                # FK 위치에 따라 Principal/Dependent 결정 (fk_field 정보 활용)
                fk_field_name = rel.get("fk_field")
                if fk_field_name:
                     # FK 이름이 'LeftId' 형태면 Left가 Principal, 아니면 Right가 Principal (간단한 추정)
                     principal_class = left_pascal if fk_field_name.lower().startswith(left_original.lower()) else right_pascal
                     dependent_class = right_pascal if principal_class == left_pascal else left_pascal
                     fk_field_pascal = safe_pascal_case(fk_field_name)
                else:
                     # FK 정보 없으면 가정 필요 (예: right가 dependent)
                     principal_class = left_pascal
                     dependent_class = right_pascal
                     fk_field_pascal = safe_pascal_case(f"{principal_class}Id")
                     print(f"[CONTEXT WARN] 1:1 relation {principal_class}-{dependent_class} - FK info missing, assuming FK '{fk_field_pascal}' on {dependent_class}.")

                key = (dependent_class, principal_class, fk_field_pascal)
                if key in generated_relations: continue
                generated_relations.add(key)

                # 네비게이션 속성 이름 (규칙 기반)
                single_nav_prop_on_dep = principal_class
                single_nav_prop_on_pri = dependent_class

                fluent_lines.append(f"        // 1:1 Relationship: {principal_class} <-> {dependent_class}")
                fluent_lines.append(f"        modelBuilder.Entity<{dependent_class}Entity>()")
                fluent_lines.append(f"            .HasOne(e => e.{single_nav_prop_on_dep})")
                fluent_lines.append(f"            .WithOne(e => e.{single_nav_prop_on_pri})")
                fluent_lines.append(f"            .HasForeignKey<{dependent_class}Entity>(e => e.{fk_field_pascal});")


            # N:M 관계 (순수 조인 테이블 사용 - UsingEntity)
            elif rel_type_normalized in ["o{--o{", "o|--o{"]:
                entity_A = left_pascal
                entity_B = right_pascal
                key = tuple(sorted((entity_A, entity_B))) # 중복 방지 키

                if key in generated_nm_pairs: continue
                generated_nm_pairs.add(key)

                # 두 엔티티를 연결하는 순수 조인 테이블 이름 찾기
                actual_join_table_name = None
                for pure_table_name in pure_join_tables: # 전달받은 순수 조인 테이블 목록 사용
                    info = entity_fields_map.get(pure_table_name, {})
                    fields = info.get('fields', [])
                    if len(fields) == 2:
                        # 필드가 참조하는 엔티티 이름 추출 (PascalCase)
                        refs = set()
                        for f in fields:
                            target_entity = None
                            # 'ref: > Entity.id' 형태에서 Entity 추출
                            meta = f.get('attributes_meta', '')
                            ref_match = re.search(r'ref\s*:\s*>\s*(\w+)', meta)
                            if ref_match:
                                target_entity = safe_pascal_case(ref_match.group(1))
                            # Fallback: 타입 이름으로 추정 (덜 정확)
                            elif 'ref' in [a.lower() for a in f.get('attributes', [])]:
                                target_entity = safe_pascal_case(f['type'])

                            if target_entity: refs.add(target_entity)

                        # 현재 관계의 양쪽 엔티티(A, B)를 모두 참조하는지 확인
                        if refs == {entity_A, entity_B}:
                            actual_join_table_name = pure_table_name # ERD 상의 실제 테이블 이름
                            break

                if actual_join_table_name:
                    # 네비게이션 속성 이름 (규칙 기반)
                    nav_prop_B_on_A = pluralize(entity_B)
                    nav_prop_A_on_B = pluralize(entity_A)
                    fluent_lines.append(f"        // N:M Relationship (Implicit): {entity_A} <-> {entity_B} via {actual_join_table_name}")
                    fluent_lines.append(f"        modelBuilder.Entity<{entity_A}Entity>()")
                    fluent_lines.append(f"            .HasMany(e => e.{nav_prop_B_on_A})")
                    fluent_lines.append(f"            .WithMany(e => e.{nav_prop_A_on_B})")
                    fluent_lines.append(f"            .UsingEntity(j => j.ToTable(\"{actual_join_table_name}\"));") # 실제 조인 테이블 이름 사용
                else:
                    # 순수 조인 테이블 못 찾음 (페이로드 조인 테이블이거나, 파싱/관계 정의 오류 가능성)
                    # 페이로드 조인 테이블은 1:N 관계 두 개로 설정되어야 함 (별도 처리 필요)
                    fluent_lines.append(f"        // [WARN CONTEXT] N:M relation {entity_A}<->{entity_B} - Could not find matching pure join table. Manual configuration needed if using payload join table.")


            # --- 기타/미지원 타입 ---
            else:
                fluent_lines.append(f"        // [UNHANDLED Relation] Normalized: {rel_type_normalized} Original: {rel_type_original} {left_pascal} <-> {right_pascal}")


    # --- 2.3. 자체 참조 관계 설정 ---
    print("[CONTEXT] Generating self-referencing configurations...")
    if self_ref_relations:
        seen_self_refs = set()
        for sr in self_ref_relations:
            entity = sr["entity"]
            parent_prop = sr["parent_prop"]
            children_prop = sr["children_prop"]
            parent_id_prop = sr["parent_id_prop"] # FK 속성 이름

            key = (entity, parent_prop, children_prop, parent_id_prop)
            if key in seen_self_refs: continue
            seen_self_refs.add(key)

            fluent_lines.append(f"        // Self-referencing: {entity}")
            fluent_lines.append(f"        modelBuilder.Entity<{entity}Entity>()")
            fluent_lines.append(f"            .HasOne(e => e.{parent_prop})")    # 참조하는 부모 속성
            fluent_lines.append(f"            .WithMany(e => e.{children_prop})") # 참조되는 자식 컬렉션 속성
            fluent_lines.append(f"            .HasForeignKey(e => e.{parent_id_prop});") # 외래 키 속성


    # --- 3. 최종 코드 조합 ---
    fluent_code = "\n".join(fluent_lines) if fluent_lines else "        // No specific configurations needed via Fluent API based on parsed relations."

    # AppDbContext C# 코드 템플릿
    code = f'''using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
// 필요한 네임스페이스들을 동적으로 추가하거나 미리 정의
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Details;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using Server.Infrastructure.Entities.Refs;
using Server.Infrastructure.Entities.UserContents; // UserContent 네임스페이스 예시
using Server.Enums; // Enum 네임스페이스

namespace {ns};

public class AppDbContext : DbContext
{{
    public AppDbContext() {{}} // 마이그레이션용 기본 생성자
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {{}}

    // DbSet 속성들
{dbsets}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {{
        base.OnModelCreating(modelBuilder);

        // Fluent API 설정
{fluent_code}
    }}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {{
        if (!optionsBuilder.IsConfigured)
        {{
            // 디자인 타임 또는 기본 구성용 연결 문자열
            string? connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {{
                // 기본값 또는 오류 처리 (예: 개발용 로컬 DB)
                connectionString = "Host=localhost;Port=5432;Database=db;Username=user;Password=password;";
                Console.WriteLine("[WARN] POSTGRES_CONNECTION_STRING environment variable not set. Using default connection string.");
            }}
            optionsBuilder.UseNpgsql(connectionString);
        }}
    }}
}}
'''
    # --- 4. 파일 쓰기 ---
    file_path = os.path.normpath(os.path.join(output_dir, "AppDbContext.cs"))
    print(f"[CONTEXT] Attempting to write AppDbContext to: {file_path}")
    write_file(file_path, code)
    print(f"[CONTEXT] Finished writing AppDbContext.")