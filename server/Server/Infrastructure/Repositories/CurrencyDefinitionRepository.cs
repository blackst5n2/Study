using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class CurrencyDefinitionRepository : ICurrencyDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CurrencyDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CurrencyDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.CurrencyDefinitions.FindAsync(id);
        return _mapper.Map<CurrencyDefinition>(infraEntity);
    }

    public async Task AddAsync(CurrencyDefinition entity)
    {
        var infraEntity = _mapper.Map<CurrencyDefinitionEntity>(entity);
        _context.CurrencyDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(CurrencyDefinition entity)
    {
        var infraEntity = _mapper.Map<CurrencyDefinitionEntity>(entity);
        _context.CurrencyDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.CurrencyDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.CurrencyDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<CurrencyDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.CurrencyDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<CurrencyDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(CurrencyDefinition entity)
    {
        var infraEntity = _mapper.Map<CurrencyDefinitionEntity>(entity);
        _context.CurrencyDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(CurrencyDefinition entity)
    {
        var infraEntity = _mapper.Map<CurrencyDefinitionEntity>(entity);
        _context.CurrencyDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.CurrencyDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.CurrencyDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}