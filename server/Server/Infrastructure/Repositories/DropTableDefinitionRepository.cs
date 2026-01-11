using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class DropTableDefinitionRepository : IDropTableDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public DropTableDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DropTableDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.DropTableDefinitions.FindAsync(id);
        return _mapper.Map<DropTableDefinition>(infraEntity);
    }

    public async Task AddAsync(DropTableDefinition entity)
    {
        var infraEntity = _mapper.Map<DropTableDefinitionEntity>(entity);
        _context.DropTableDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(DropTableDefinition entity)
    {
        var infraEntity = _mapper.Map<DropTableDefinitionEntity>(entity);
        _context.DropTableDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.DropTableDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.DropTableDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<DropTableDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.DropTableDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<DropTableDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(DropTableDefinition entity)
    {
        var infraEntity = _mapper.Map<DropTableDefinitionEntity>(entity);
        _context.DropTableDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(DropTableDefinition entity)
    {
        var infraEntity = _mapper.Map<DropTableDefinitionEntity>(entity);
        _context.DropTableDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.DropTableDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.DropTableDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}