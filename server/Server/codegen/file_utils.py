"""
파일/폴더 생성, 경로 유틸, 템플릿 처리 모듈
"""
import os
import re

def pascal_case(s):
    """snake_case, lowerCamelCase, UPPER_CASE 등 → PascalCase (C# 스타일, 모든 입력 케이스 대응)"""
    words = re.split(r'[_\s]+', s.strip())
    return ''.join(w.lower().capitalize() for w in words if w)

def ensure_dir(path):
    """필요한 디렉터리 생성"""
    os.makedirs(path, exist_ok=True)

def write_file(path, content):
    """파일에 내용 저장 (Server/를 기준으로 상대경로 사용, test/ 하위로 강제하지 않음)"""
    dir_path = os.path.dirname(path)
    if dir_path:
        ensure_dir(dir_path)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)

