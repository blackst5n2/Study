"""
ValueObject 코드 생성 모듈
"""
def generate_value_object(name, fields, output_dir):
    """ValueObject C# record 타입 생성 및 파일 저장 (이름은 이미 PascalCase로 변환되어 있음)"""
    from codegen.file_utils import write_file
    props = ", ".join([f["type"] + " " + f["name"] for f in fields])
    code = f"""namespace Server.Core.ValueObjects;

public record {name}({props});

"""
    # 파일명도 PascalCase가 되도록 name 그대로 사용
    file_path = f"{output_dir}/{name}.cs"
    write_file(file_path, code)
