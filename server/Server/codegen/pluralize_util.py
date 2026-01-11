"""
C# 스타일 영어 복수형 변환 유틸 (단순 규칙 + 예외)
"""
def pluralize(word):
    irregulars = {
        'Category': 'Categories',
        'Inventory': 'Inventories',
        'Entity': 'Entities',
        'Tag': 'Tags',
        'Quest': 'Quests',
        'Requirement': 'Requirements',
        'Player': 'Players',
        'Guild': 'Guilds',
        'Item': 'Items',
        'Objective': 'Objectives',
        'Reward': 'Rewards',
    }
    if word in irregulars:
        return irregulars[word]
    # 이미 복수형이면 그대로 반환
    if word.endswith('ies') or word.endswith('es') or (word.endswith('s') and not word.endswith('ss')):
        return word
    if word.endswith('y') and word[-2] not in 'aeiou':
        return word[:-1] + 'ies'
    elif word.endswith('s') or word.endswith('x') or word.endswith('z') or word.endswith('ch') or word.endswith('sh'):
        return word + 'es'
    else:
        return word + 's'
