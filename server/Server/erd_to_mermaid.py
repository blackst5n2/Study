import re
import sys
from collections import defaultdict

# Usage: python erd_to_mermaid.py erd.mmd > erd_full.mmd

def parse_erd(filename):
    tables = set()
    relations = []
    current_table = None
    refs = defaultdict(list)
    with open(filename, encoding='utf-8') as f:
        for line in f:
            # table TableName {
            m_table = re.match(r'\s*table\s+(\w+)\s*{', line)
            if m_table:
                current_table = m_table.group(1)
                tables.add(current_table)
                continue
            #   field_name type [ref: > OtherTable.field]
            m_ref = re.search(r'\[ref: >\s*(\w+)\.(\w+)', line)
            if current_table and m_ref:
                to_table = m_ref.group(1)
                # Save relation: current_table -- to_table
                relations.append((current_table, to_table))
                refs[current_table].append(to_table)
            # End of table
            if '}' in line and current_table:
                current_table = None
    return tables, relations

def to_mermaid(tables, relations):
    print('erDiagram')
    for from_table, to_table in sorted(set(relations)):
        print(f'  {to_table} ||--o{{ {from_table}')

if __name__ == '__main__':
    if len(sys.argv) < 2:
        print('Usage: python erd_to_mermaid.py erd.mmd > erd_full.mmd')
        sys.exit(1)
    tables, relations = parse_erd(sys.argv[1])
    to_mermaid(tables, relations)
