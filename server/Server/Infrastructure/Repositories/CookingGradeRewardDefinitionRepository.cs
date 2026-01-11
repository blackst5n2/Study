using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class CookingGradeRewardDefinitionRepository : ICookingGradeRewardDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CookingGradeRewardDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CookingGradeRewardDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.CookingGradeRewardDefinitions.FindAsync(id);
        return _mapper.Map<CookingGradeRewardDefinition>(infraEntity);
    }

    public async Task AddAsync(CookingGradeRewardDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingGradeRewardDefinitionEntity>(entity);
        _context.CookingGradeRewardDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(CookingGradeRewardDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingGradeRewardDefinitionEntity>(entity);
        _context.CookingGradeRewardDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.CookingGradeRewardDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.CookingGradeRewardDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}