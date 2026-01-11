"""
Repository 코드 생성 모듈
"""
import os
from codegen.util import safe_pascal_case
from codegen.pluralize_util import pluralize

def generate_repository_interface(entity_name, pk_type, domain_ns, output_dir, code_unique=False):
    entity_name = safe_pascal_case(entity_name)
    """Repository 인터페이스 코드 생성 및 파일 저장 (이름은 이미 PascalCase로 변환되어 있음)
    code_unique: Code(string) 컬럼이 unique인 경우 True
    """
    from codegen.file_utils import write_file
    code = f'''using {domain_ns};

namespace Server.Core.Repositories;

public interface I{safe_pascal_case(entity_name)}Repository
{{
    Task<{safe_pascal_case(entity_name)}?> GetByIdAsync({pk_type} id);
    Task AddAsync({safe_pascal_case(entity_name)} entity);
    Task SaveAsync({safe_pascal_case(entity_name)} entity);
    Task DeleteAsync({pk_type} id);
'''
    if code_unique:
        code += f'''    Task<{safe_pascal_case(entity_name)}?> GetByCodeAsync(string code);
    Task AddByCodeAsync({safe_pascal_case(entity_name)} entity);
    Task SaveByCodeAsync({safe_pascal_case(entity_name)} entity);
    Task DeleteByCodeAsync(string code);
'''
    code += "}"
    file_path = os.path.normpath(f"{output_dir}/I{safe_pascal_case(entity_name)}Repository.cs")
    write_file(file_path, code)

def generate_repository_impl(entity_name, pk_type, domain_ns, infra_ns, output_dir, code_unique=False):
    entity_name = safe_pascal_case(entity_name)
    """Repository 구현체 코드 생성 및 파일 저장 (이름은 이미 PascalCase로 변환되어 있음)
    code_unique: Code(string) 컬럼이 unique인 경우 True
    """
    ns_length = len(domain_ns.split("."))
    if ns_length == 3:
        ns = domain_ns.split(".")[2]
    else:
        ns = domain_ns.split(".")[2]+"."+domain_ns.split(".")[3]
    from codegen.file_utils import write_file
    code = f'''using {domain_ns};
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.{ns};

namespace Server.Infrastructure.Repositories;

public class {safe_pascal_case(entity_name)}Repository : I{safe_pascal_case(entity_name)}Repository
{{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public {safe_pascal_case(entity_name)}Repository(AppDbContext context, IMapper mapper)
    {{
        _context = context;
        _mapper = mapper;
    }}

    public async Task<{safe_pascal_case(entity_name)}?> GetByIdAsync({pk_type} id)
    {{
        var infraEntity = await _context.{safe_pascal_case(pluralize(entity_name))}.FindAsync(id);
        return _mapper.Map<{safe_pascal_case(entity_name)}>(infraEntity);
    }}

    public async Task AddAsync({safe_pascal_case(entity_name)} entity)
    {{
        var infraEntity = _mapper.Map<{safe_pascal_case(entity_name)}Entity>(entity);
        _context.{safe_pascal_case(pluralize(entity_name))}.Add(infraEntity);
        await _context.SaveChangesAsync();
    }}

    public async Task SaveAsync({safe_pascal_case(entity_name)} entity)
    {{
        var infraEntity = _mapper.Map<{safe_pascal_case(entity_name)}Entity>(entity);
        _context.{safe_pascal_case(pluralize(entity_name))}.Update(infraEntity);
        await _context.SaveChangesAsync();
    }}

    public async Task DeleteAsync({pk_type} id)
    {{
        var infraEntity = await _context.{safe_pascal_case(pluralize(entity_name))}.FindAsync(id);
        if (infraEntity != null)
        {{
            _context.{safe_pascal_case(pluralize(entity_name))}.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }}
    }}
'''
    if code_unique:
        code += f'''
    public async Task<{safe_pascal_case(entity_name)}?> GetByCodeAsync(string code)
    {{
        var infraEntity = await _context.{safe_pascal_case(pluralize(entity_name))}.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<{safe_pascal_case(entity_name)}>(infraEntity);
    }}
    public async Task AddByCodeAsync({safe_pascal_case(entity_name)} entity)
    {{
        var infraEntity = _mapper.Map<{safe_pascal_case(entity_name)}Entity>(entity);
        _context.{safe_pascal_case(pluralize(entity_name))}.Add(infraEntity);
        await _context.SaveChangesAsync();
    }}
    public async Task SaveByCodeAsync({safe_pascal_case(entity_name)} entity)
    {{
        var infraEntity = _mapper.Map<{safe_pascal_case(entity_name)}Entity>(entity);
        _context.{safe_pascal_case(pluralize(entity_name))}.Update(infraEntity);
        await _context.SaveChangesAsync();
    }}
    public async Task DeleteByCodeAsync(string code)
    {{
        var infraEntity = await _context.{safe_pascal_case(pluralize(entity_name))}.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {{
            _context.{safe_pascal_case(pluralize(entity_name))}.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }}
    }}
'''
    code += "}"
    file_path = os.path.normpath(f"{output_dir}/{safe_pascal_case(entity_name)}Repository.cs")
    write_file(file_path, code)
