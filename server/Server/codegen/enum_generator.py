"""
ERD에서 추출한 enums 딕셔너리로 C# enum 코드 파일을 생성
"""
import os

def to_pascal_case(name):
    import re
    if not name:
        return ''
    s = re.sub(r'[_\-]', ' ', name)
    words = re.findall(r'[A-Z]?[a-z]+|[A-Z]+(?![a-z])|\d+', s)
    return ''.join(word.capitalize() for word in words if word)

def generate_enum_cs(enum_name, values, output_dir):
    enum_name_pc = to_pascal_case(enum_name)
    lines = [
        f"namespace Server.Enums\n{{",
        f"    public enum {enum_name_pc}",
        "    {"
    ]
    for v in values:
        lines.append(f"        {to_pascal_case(v)},")
    if values:
        lines[-1] = lines[-1].rstrip(',')  # 마지막 콤마 제거
    lines.append("    }")
    lines.append("}")
    code = '\n'.join(lines)
    os.makedirs(output_dir, exist_ok=True)
    out_path = os.path.join(output_dir, f"{enum_name_pc}.cs")
    with open(out_path, "w", encoding="utf-8") as f:
        f.write(code)

def generate_enums(enums, output_dir="Server/Enums"):
    for enum_name, values in enums.items():
        generate_enum_cs(enum_name, values, output_dir)
