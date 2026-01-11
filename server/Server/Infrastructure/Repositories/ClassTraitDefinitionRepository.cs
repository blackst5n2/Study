using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class ClassTraitDefinitionRepository : IClassTraitDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ClassTraitDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClassTraitDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.ClassTraitDefinitions.FindAsync(id);
        return _mapper.Map<ClassTraitDefinition>(infraEntity);
    }

    public async Task AddAsync(ClassTraitDefinition entity)
    {
        var infraEntity = _mapper.Map<ClassTraitDefinitionEntity>(entity);
        _context.ClassTraitDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(ClassTraitDefinition entity)
    {
        var infraEntity = _mapper.Map<ClassTraitDefinitionEntity>(entity);
        _context.ClassTraitDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.ClassTraitDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.ClassTraitDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}