using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class SkillDefinitionRepository : ISkillDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SkillDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SkillDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.SkillDefinitions.FindAsync(id);
        return _mapper.Map<SkillDefinition>(infraEntity);
    }

    public async Task AddAsync(SkillDefinition entity)
    {
        var infraEntity = _mapper.Map<SkillDefinitionEntity>(entity);
        _context.SkillDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(SkillDefinition entity)
    {
        var infraEntity = _mapper.Map<SkillDefinitionEntity>(entity);
        _context.SkillDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.SkillDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.SkillDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<SkillDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.SkillDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<SkillDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(SkillDefinition entity)
    {
        var infraEntity = _mapper.Map<SkillDefinitionEntity>(entity);
        _context.SkillDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(SkillDefinition entity)
    {
        var infraEntity = _mapper.Map<SkillDefinitionEntity>(entity);
        _context.SkillDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.SkillDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.SkillDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}