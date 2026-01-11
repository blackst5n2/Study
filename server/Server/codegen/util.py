import re

def to_pascal_case(name):
    if not name:
        return ''
    s = re.sub(r'[_\-]', ' ', name)
    words = re.findall(r'[A-Z]?[a-z]+|[A-Z]+(?![a-z])|\d+', s)
    def fix_word(word):
        if word.isupper() and len(word) > 1:
            return word[0].upper() + word[1:].lower()
        return word.capitalize()
    return ''.join(fix_word(word) for word in words if word)

def is_pascal_case(name):
    # Returns True if the string is already PascalCase (first letter uppercase, no underscores, not all uppercase)
    return name and name[0].isupper() and '_' not in name and not name.isupper() and not name.islower()

def safe_pascal_case(name):
    # GMRole, GMActionLog 등은 GmRole, GmActionLog로 변환
    if re.match(r'^[A-Z]{2,}', name):
        # 두 글자 이상 대문자 접두어는 첫 글자만 대문자로, 나머지는 소문자 + 다음 단어는 대문자
        # 예: GMRole -> GmRole, GMAccount -> GmAccount, GMActionLog -> GmActionLog
        # 1. 앞의 연속 대문자 추출
        m = re.match(r'^([A-Z]+)([A-Z][a-z0-9].*)$', name)
        if m:
            prefix, rest = m.groups()
            # prefix: 'GM', rest: 'Role' or 'ActionLog'
            new_name = prefix[0] + prefix[1:].lower() + rest
            return to_pascal_case(new_name)
        else:
            # 예외: 전부 대문자(ex: 'GM')면 첫 글자만 대문자로
            return name[0] + name[1:].lower() if len(name) > 1 else name.upper()
    return name if is_pascal_case(name) else to_pascal_case(name)

def normalize_reltype(reltype):
    mapping = {
        '}o--||': '||--o{',  # 1:N
        '{o--||': '||--o{',  # 1:N
        '|o--||': '||--o{',  # 1:N
        '}o--o|': 'o|--o{',  # N:M
        '{o--o|': 'o|--o{',  # N:M
        # 필요한 만큼 추가
    }
    reltype_trim = reltype.strip()
    return mapping.get(reltype_trim, reltype_trim)