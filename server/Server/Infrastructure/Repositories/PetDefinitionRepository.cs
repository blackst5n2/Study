using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class PetDefinitionRepository : IPetDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PetDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PetDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.PetDefinitions.FindAsync(id);
        return _mapper.Map<PetDefinition>(infraEntity);
    }

    public async Task AddAsync(PetDefinition entity)
    {
        var infraEntity = _mapper.Map<PetDefinitionEntity>(entity);
        _context.PetDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(PetDefinition entity)
    {
        var infraEntity = _mapper.Map<PetDefinitionEntity>(entity);
        _context.PetDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.PetDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.PetDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PetDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.PetDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<PetDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(PetDefinition entity)
    {
        var infraEntity = _mapper.Map<PetDefinitionEntity>(entity);
        _context.PetDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(PetDefinition entity)
    {
        var infraEntity = _mapper.Map<PetDefinitionEntity>(entity);
        _context.PetDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.PetDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.PetDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}