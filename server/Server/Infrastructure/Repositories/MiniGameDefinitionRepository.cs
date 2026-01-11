using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class MiniGameDefinitionRepository : IMiniGameDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MiniGameDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MiniGameDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.MiniGameDefinitions.FindAsync(id);
        return _mapper.Map<MiniGameDefinition>(infraEntity);
    }

    public async Task AddAsync(MiniGameDefinition entity)
    {
        var infraEntity = _mapper.Map<MiniGameDefinitionEntity>(entity);
        _context.MiniGameDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(MiniGameDefinition entity)
    {
        var infraEntity = _mapper.Map<MiniGameDefinitionEntity>(entity);
        _context.MiniGameDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.MiniGameDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.MiniGameDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<MiniGameDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.MiniGameDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<MiniGameDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(MiniGameDefinition entity)
    {
        var infraEntity = _mapper.Map<MiniGameDefinitionEntity>(entity);
        _context.MiniGameDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(MiniGameDefinition entity)
    {
        var infraEntity = _mapper.Map<MiniGameDefinitionEntity>(entity);
        _context.MiniGameDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.MiniGameDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.MiniGameDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}