using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class BuffDefinitionRepository : IBuffDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BuffDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BuffDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.BuffDefinitions.FindAsync(id);
        return _mapper.Map<BuffDefinition>(infraEntity);
    }

    public async Task AddAsync(BuffDefinition entity)
    {
        var infraEntity = _mapper.Map<BuffDefinitionEntity>(entity);
        _context.BuffDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(BuffDefinition entity)
    {
        var infraEntity = _mapper.Map<BuffDefinitionEntity>(entity);
        _context.BuffDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.BuffDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.BuffDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<BuffDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.BuffDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<BuffDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(BuffDefinition entity)
    {
        var infraEntity = _mapper.Map<BuffDefinitionEntity>(entity);
        _context.BuffDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(BuffDefinition entity)
    {
        var infraEntity = _mapper.Map<BuffDefinitionEntity>(entity);
        _context.BuffDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.BuffDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.BuffDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}