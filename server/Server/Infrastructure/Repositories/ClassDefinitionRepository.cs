using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class ClassDefinitionRepository : IClassDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ClassDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClassDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.ClassDefinitions.FindAsync(id);
        return _mapper.Map<ClassDefinition>(infraEntity);
    }

    public async Task AddAsync(ClassDefinition entity)
    {
        var infraEntity = _mapper.Map<ClassDefinitionEntity>(entity);
        _context.ClassDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(ClassDefinition entity)
    {
        var infraEntity = _mapper.Map<ClassDefinitionEntity>(entity);
        _context.ClassDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.ClassDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.ClassDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ClassDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.ClassDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<ClassDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(ClassDefinition entity)
    {
        var infraEntity = _mapper.Map<ClassDefinitionEntity>(entity);
        _context.ClassDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(ClassDefinition entity)
    {
        var infraEntity = _mapper.Map<ClassDefinitionEntity>(entity);
        _context.ClassDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.ClassDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.ClassDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}