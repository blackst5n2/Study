using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class GuildLevelDefinitionRepository : IGuildLevelDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GuildLevelDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GuildLevelDefinition?> GetByIdAsync(int id)
    {
        var infraEntity = await _context.GuildLevelDefinitions.FindAsync(id);
        return _mapper.Map<GuildLevelDefinition>(infraEntity);
    }

    public async Task AddAsync(GuildLevelDefinition entity)
    {
        var infraEntity = _mapper.Map<GuildLevelDefinitionEntity>(entity);
        _context.GuildLevelDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(GuildLevelDefinition entity)
    {
        var infraEntity = _mapper.Map<GuildLevelDefinitionEntity>(entity);
        _context.GuildLevelDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var infraEntity = await _context.GuildLevelDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.GuildLevelDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}