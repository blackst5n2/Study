using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class PartyDefinitionRepository : IPartyDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PartyDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PartyDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.PartyDefinitions.FindAsync(id);
        return _mapper.Map<PartyDefinition>(infraEntity);
    }

    public async Task AddAsync(PartyDefinition entity)
    {
        var infraEntity = _mapper.Map<PartyDefinitionEntity>(entity);
        _context.PartyDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(PartyDefinition entity)
    {
        var infraEntity = _mapper.Map<PartyDefinitionEntity>(entity);
        _context.PartyDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.PartyDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.PartyDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}