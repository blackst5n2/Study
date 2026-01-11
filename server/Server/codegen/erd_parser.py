# erd_parser.py

import re

# 타입 매핑 (WIW 스타일) - 이 부분은 그대로 유지됩니다.
TYPE_MAP = {
    "uuid": "Guid",
    "guid": "Guid",
    "string": "string",
    "int": "int",
    "bool": "bool",
    "datetime": "DateTime",
    "float": "float",
    "double": "double"
}

def map_type(ftype, enums, nullable=False):
    # 이 함수는 그대로 유지됩니다.
    if ftype in enums:
        return ftype
    ftype_norm = str(ftype).lower()
    cstype = TYPE_MAP.get(ftype_norm, TYPE_MAP.get(ftype, "string"))
    return f"{cstype}?" if nullable and cstype not in ["string", "Guid"] else cstype

def classify_entity_category(tname):
    # 이 함수는 그대로 유지됩니다.
    if tname.endswith("Log"):
        return "Logs"
    elif tname.endswith("Detail") or tname.endswith("Requirement") or tname.endswith("Probability") or tname.endswith("AvailableTime"):
        return "Details"
    elif tname.endswith("Definition") or tname.endswith("Product") or tname.endswith("Effect") or tname.endswith("Rune") or tname.endswith("Enhancement") or tname.endswith("Option"):
        return "Definitions"
    elif "Allowed" in tname or "Ref" in tname or "Map" in tname or "Link" in tname:
        return "Refs"
    elif tname.startswith("Player") or tname.endswith("Instance") or tname.endswith("Progress") or tname.endswith("Achievement") or tname.endswith("Quest"):
        return "Progress"
    else:
        return "General"

# <<< --- 수정된 parse_erd 함수 시작 --- >>>
def parse_erd(erd_path):
    tables, enums, relations = {}, {}, []
    current, in_enum = None, None
    with open(erd_path, encoding="utf-8") as f:
        in_indexes = False
        for line in f:
            line = line.strip() # 앞/뒤 공백 먼저 제거

            table_match = re.match(r'\s*(table|entity)\s+(\w+)\s*{', line, re.I)
            enum_match = re.match(r'\s*enum\s+(\w+)\s*{', line)
            # 관계 파싱 정규표현식: 실제 ERD 라인과 완전 일치하도록 개선
            # 예: JobSkillTree }o--|| JobSkillDefinition : "FK_JobSkillTree_parent_skill_id"
            rel_match = re.match(r'^\s*(\w+)\s+([\}\{\|o\-]+)\s+(\w+)(?:\s*:\s*"([^"]*)")?.*$', line)
            # 필드 파싱 정규표현식은 유지
            field_match = re.match(r'\s*(\w+)\s+(\w+)(\?)?(?:\s+(.+))?', line)

            if enum_match:
                in_enum = enum_match.group(1)
                enums[in_enum] = []
                # enum 정의 라인('{')에 값이 있을 경우 처리
                line_content_after_bracket = line.split('{', 1)[1].strip()
                if line_content_after_bracket:
                     # 주석(//, /*, #) 처리 추가
                    values = [v.strip() for v in line_content_after_bracket.split(',') if v.strip() and not v.strip().startswith(("//", "/*", "#"))]
                    if values:
                        enums[in_enum].extend(values)

            elif in_enum and '}' in line: # enum 블록 종료
                # 닫는 괄호 라인('}') 이전의 값 처리
                line_content_before_bracket = line.split('}', 1)[0].strip()
                if line_content_before_bracket:
                    # 주석(//, /*, #) 처리 추가
                    values = [v.strip() for v in line_content_before_bracket.split(',') if v.strip() and not v.strip().startswith(("//", "/*", "#"))]
                    if values:
                        enums[in_enum].extend(values)
                in_enum = None # enum 파싱 종료

            elif in_enum: # enum 블록 내부 값 처리 (콤마 구분)
                # 주석(//, /*, #) 처리 추가
                values = [v.strip() for v in line.split(',') if v.strip() and not v.strip().startswith(("//", "/*", "#"))]
                if values:
                    enums[in_enum].extend(values)

            elif table_match:
                current = table_match.group(2)
                tables[current] = {"fields": [], "pk_fields": [], "indexes": []}
                in_indexes = False
            elif '}' in line and current: # 테이블 블록 종료
                current = None
                in_indexes = False
            elif current and line.startswith("indexes"):
                in_indexes = True
                continue
            elif current and in_indexes:
                if line == "}":
                    in_indexes = False
                    continue
                # 파싱: (a, b) [unique]
                m = re.match(r'\(([^)]+)\)\s*(\[.*\])?', line)
                if m:
                    columns = [col.strip() for col in m.group(1).split(",")]
                    attr = m.group(2) or ""
                    is_unique = "unique" in attr.lower()
                    tables[current]["indexes"].append({
                        "columns": columns,
                        "unique": is_unique
                    })
            elif current and field_match:
                # 이 필드 파싱 부분은 기존 코드와 동일하게 유지됩니다.
                fname, ftype, nullable, meta = field_match.groups()
                fname_norm = fname.strip().lower()
                ftype_norm = ftype.strip().lower()
                is_code_field = fname_norm == "code" and ftype_norm == "string"
                meta = meta or ""
                key = None
                attributes = []
                if "[" in meta and "]" in meta:
                    attr_block = meta[meta.find("[")+1:meta.find("]")]
                    for part in attr_block.split(","):
                        part = part.strip()
                        part_l = part.lower()
                        if part_l.startswith("ref"):
                            attributes.append("ref")
                        elif part_l == "pk":
                            key = "PK"
                            tables[current]["pk_fields"].append(fname)
                            attributes.append("pk")
                        elif part_l == "fk":
                            key = "FK"
                            attributes.append("fk")
                        elif part_l == "unique":
                            attributes.append("unique")
                        elif part_l == "required":
                            attributes.append("required")
                        elif part_l.startswith("maxlength"):
                            attributes.append(part)
                        elif part_l == "index":
                            attributes.append("index")
                        elif part_l == "null":
                            attributes.append("null") # nullable 속성 처리
                        # 'default' 속성 파싱 추가
                        elif part_l.startswith("default"):
                             attributes.append(part) # default:[value] 또는 default:`sql_value`
                else:
                    # fallback: old logic for non-bracket attributes
                    meta_tokens = re.findall(r'\w+|`[^`]*`|\'[^\']*\'|\"[^\"]*\"', meta) # 값 포함하여 추출 개선
                    for token in meta_tokens:
                        token_l = token.lower()
                        if token_l == "pk":
                            key = "PK"
                            tables[current]["pk_fields"].append(fname)
                            attributes.append("pk")
                        elif token_l == "fk":
                            key = "FK"
                            attributes.append("fk")
                        elif token_l == "required":
                            attributes.append("required")
                        elif token_l == "unique":
                            attributes.append("unique")
                        elif token.startswith("MaxLength"):
                            m = re.match(r'MaxLength\((\d+)\)', token, re.I)
                            if m:
                                attributes.append(f"MaxLength({m.group(1)})")
                        elif token_l == "index":
                            attributes.append("index")
                        elif token_l == "null": # nullable 속성 처리
                             attributes.append("null")
                        # 'default' 속성 파싱 추가 (fallback)
                        elif token_l.startswith("default"):
                             attributes.append(token)

                # nullable 플래그 설정: 명시적 ? 또는 null 속성
                is_nullable = bool(nullable) or "null" in attributes

                if is_code_field:
                    fname = "Code"
                    ftype = "string"

                tables[current]["fields"].append({
                    "type": ftype, "name": fname, "nullable": is_nullable, "key": key, "attributes": attributes, "attributes_meta": meta
                })

            # 관계 파싱 (rel_match 정규표현식 수정됨)
            elif rel_match:
                left, rel, right, label = rel_match.groups()
                # left 테이블의 모든 ref 필드 중 [ref: > TargetEntity.pk]의 TargetEntity가 right와 정확히 일치하는 경우마다 별도의 relation 생성
                if left in tables:
                    for f in tables[left]["fields"]:
                        if "ref" in f.get("attributes", []):
                            # ref 메타에서 TargetEntity 추출
                            ref_target = None
                            # 메타에서 [ref: > TargetEntity.pk] 패턴 추출
                            meta = f.get("attributes_meta", "") if "attributes_meta" in f else ""
                            ref_match = re.search(r'ref\s*:\s*>\s*(\w+)', meta)
                            if ref_match:
                                ref_target = ref_match.group(1)
                            else:
                                # fallback: 타입명에서 추정
                                ref_target = f["type"]
                            if ref_target and ref_target.lower() == right.lower():
                                relations.append({
                                    "left": left,
                                    "rel": rel,
                                    "right": right,
                                    "label": (label or '').strip(),
                                    "fk_field": f["name"]
                                })

    # 파싱 후 enum 값 정리: 리스트 내 중복 제거 및 정렬 (선택적)
    for enum_name in enums:
        enums[enum_name] = sorted(list(set(enums[enum_name])))
    
    return tables, enums, relations
# <<< --- 수정된 parse_erd 함수 끝 --- >>>