using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class DialogueDefinitionRepository : IDialogueDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public DialogueDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DialogueDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.DialogueDefinitions.FindAsync(id);
        return _mapper.Map<DialogueDefinition>(infraEntity);
    }

    public async Task AddAsync(DialogueDefinition entity)
    {
        var infraEntity = _mapper.Map<DialogueDefinitionEntity>(entity);
        _context.DialogueDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(DialogueDefinition entity)
    {
        var infraEntity = _mapper.Map<DialogueDefinitionEntity>(entity);
        _context.DialogueDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.DialogueDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.DialogueDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<DialogueDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.DialogueDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<DialogueDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(DialogueDefinition entity)
    {
        var infraEntity = _mapper.Map<DialogueDefinitionEntity>(entity);
        _context.DialogueDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(DialogueDefinition entity)
    {
        var infraEntity = _mapper.Map<DialogueDefinitionEntity>(entity);
        _context.DialogueDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.DialogueDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.DialogueDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}