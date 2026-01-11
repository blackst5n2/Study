using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class SkillEffectDefinitionRepository : ISkillEffectDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SkillEffectDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SkillEffectDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.SkillEffectDefinitions.FindAsync(id);
        return _mapper.Map<SkillEffectDefinition>(infraEntity);
    }

    public async Task AddAsync(SkillEffectDefinition entity)
    {
        var infraEntity = _mapper.Map<SkillEffectDefinitionEntity>(entity);
        _context.SkillEffectDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(SkillEffectDefinition entity)
    {
        var infraEntity = _mapper.Map<SkillEffectDefinitionEntity>(entity);
        _context.SkillEffectDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.SkillEffectDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.SkillEffectDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}