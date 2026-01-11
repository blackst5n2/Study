"""
Application Service 코드 생성 모듈
"""
import os
from codegen.repository_generator import safe_pascal_case

def generate_application_service(entity_name, repo_ns, output_dir, code_unique=False):
    entity_name = safe_pascal_case(entity_name)
    """Application Service 코드 생성 및 파일 저장 (이름은 이미 PascalCase로 변환되어 있음)"""
    from codegen.file_utils import write_file
    lines = [
        f"using Server.Core.Repositories;",
        f"using Server.Core.Entities.Entities;",
        f"using Server.Core.Entities.Definitions;",
        f"using Server.Core.Entities.Details;",
        f"using Server.Core.Entities.Logs;",
        f"using Server.Core.Entities.Progress;",
        f"using Server.Core.Entities.Refs;",
        f"using Server.Core.Entities.UserContents;",
        "",
        f"namespace Server.Application.Services;",
        "",
        f"public class {entity_name}Service",
        "{",
        f"    private readonly I{entity_name}Repository _repo;",
        f"    public {entity_name}Service(I{entity_name}Repository repo) => _repo = repo;",
        "",
        f"    public async Task<{entity_name}?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);",
        f"    public async Task AddAsync({entity_name} entity) => await _repo.AddAsync(entity);",
        f"    public async Task SaveAsync({entity_name} entity) => await _repo.SaveAsync(entity);",
        f"    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);",
    ]
    if code_unique:
        lines.append(f"    public async Task<{entity_name}?> GetByCodeAsync(string code) => await _repo.GetByCodeAsync(code);")
        lines.append(f"    public async Task AddByCodeAsync({entity_name} entity) => await _repo.AddByCodeAsync(entity);")
        lines.append(f"    public async Task SaveByCodeAsync({entity_name} entity) => await _repo.SaveByCodeAsync(entity);")
        lines.append(f"    public async Task DeleteByCodeAsync(string code) => await _repo.DeleteByCodeAsync(code);")
    lines.append("    // TODO: 유스케이스별 메서드 추가")
    lines.append("}")
    code = "\n".join(lines) + "\n"
    file_path = os.path.normpath(f"{output_dir}/{entity_name}Service.cs")
    write_file(file_path, code)
