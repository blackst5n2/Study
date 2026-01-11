"""
AutoMapper MapperProfile 코드 자동 생성 모듈
"""
def generate_automapper_profile(class_names, output_path="Application/MapperProfile.cs"):
    lines = [
        "using AutoMapper;",
        "using Server.Infrastructure.Entities;",
        "using Server.Core.Entities;",
        "using Server.Application.DTOs;",
    ]
    # 주요 분류별 네임스페이스 자동 import
    for category in ["Definitions", "Details", "Logs", "Progress", "Refs"]:
        lines.append(f"using Server.Infrastructure.Entities.{category};")
        lines.append(f"using Server.Core.Entities.{category};")
        lines.append(f"using Server.Application.DTOs.{category};")
    lines.append("")
    lines.append("namespace Server.Application {")
    lines.append("    public class MapperProfile : Profile")
    lines.append("    {")
    lines.append("        public MapperProfile()")
    lines.append("        {")
    for name in class_names:
        # Entity <-> Domain
        lines.append(f"            CreateMap<{name}Entity, {name}>().ReverseMap();")
        # Domain <-> DTO
        lines.append(f"            CreateMap<{name}, {name}Dto>().ReverseMap();")
    lines += [
        "        }",
        "    }",
        "}"
    ]
    code = "\n".join(lines)
    # test/ 하위에 출력하도록 경로 강제
    import os
    os.makedirs(os.path.dirname(output_path), exist_ok=True)
    with open(output_path, "w", encoding="utf-8") as f:
        f.write(code)
