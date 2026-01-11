using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class ContainerRepository : IContainerRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ContainerRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Container?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.Containers.FindAsync(id);
        return _mapper.Map<Container>(infraEntity);
    }

    public async Task AddAsync(Container entity)
    {
        var infraEntity = _mapper.Map<ContainerEntity>(entity);
        _context.Containers.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(Container entity)
    {
        var infraEntity = _mapper.Map<ContainerEntity>(entity);
        _context.Containers.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.Containers.FindAsync(id);
        if (infraEntity != null)
        {
            _context.Containers.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}