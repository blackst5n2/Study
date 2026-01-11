"""
DTO 코드 생성 모듈
"""
def generate_dto(entity_name, fields, output_dir, ns, enums=None):
    """DTO 코드 생성 및 파일 저장 (이름은 이미 PascalCase로 변환되어 있음)"""
    from codegen.file_utils import write_file
    from codegen.entity_generator import to_pascal_case, map_type
    enums = enums or set()
    needs_enum_using = any(f['type'] in enums for f in fields)
    usings = []
    if needs_enum_using:
        usings.append("using Server.Enums;")
    usings.append(f"namespace {ns};")
    props = []
    for f in fields:
        if f['type'] in enums:
            cstype = to_pascal_case(f['type'])
        else:
            cstype = map_type(f['type'], enums, f['nullable'])
        prop = to_pascal_case(f['name'])
        props.append(f"public {cstype} {prop} {{ get; set; }}")
    props = "\n    ".join(props)
    code = "\n".join(usings) + f"\n\npublic class {entity_name}Dto\n{{\n    {props}\n}}\n"
    import os
    file_path = os.path.normpath(f"{output_dir}/{entity_name}Dto.cs")
    os.makedirs(os.path.dirname(file_path), exist_ok=True)
    write_file(file_path, code)
