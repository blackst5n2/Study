using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class CropDefinitionRepository : ICropDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CropDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CropDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.CropDefinitions.FindAsync(id);
        return _mapper.Map<CropDefinition>(infraEntity);
    }

    public async Task AddAsync(CropDefinition entity)
    {
        var infraEntity = _mapper.Map<CropDefinitionEntity>(entity);
        _context.CropDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(CropDefinition entity)
    {
        var infraEntity = _mapper.Map<CropDefinitionEntity>(entity);
        _context.CropDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.CropDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.CropDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}