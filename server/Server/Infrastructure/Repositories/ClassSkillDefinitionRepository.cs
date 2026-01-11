using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class ClassSkillDefinitionRepository : IClassSkillDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ClassSkillDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClassSkillDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.ClassSkillDefinitions.FindAsync(id);
        return _mapper.Map<ClassSkillDefinition>(infraEntity);
    }

    public async Task AddAsync(ClassSkillDefinition entity)
    {
        var infraEntity = _mapper.Map<ClassSkillDefinitionEntity>(entity);
        _context.ClassSkillDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(ClassSkillDefinition entity)
    {
        var infraEntity = _mapper.Map<ClassSkillDefinitionEntity>(entity);
        _context.ClassSkillDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.ClassSkillDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.ClassSkillDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}