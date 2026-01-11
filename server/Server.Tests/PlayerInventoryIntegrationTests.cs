#nullable enable
using System;
using Server.Data.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Server.DTOs;
using Server.Services;

// 간단한 Mock Repository
class MockPlayerInventoryRepository : Server.Data.Repositories.IPlayerInventoryRepository
{
    private readonly List<Server.Data.Entities.PlayerInventoryEntity> _store = new();
    public Task AddAsync(Server.Data.Entities.PlayerInventoryEntity entity)
    {
        entity.Id = _store.Count + 1;
        _store.Add(entity);
        return Task.CompletedTask;
    }
    public Task<Server.Data.Entities.PlayerInventoryEntity?> GetByIdAsync(int id)
        => Task.FromResult(_store.Find(x => x.Id == id));
    public Task<IEnumerable<Server.Data.Entities.PlayerInventoryEntity>> GetAllAsync()
        => Task.FromResult<IEnumerable<Server.Data.Entities.PlayerInventoryEntity>>(_store);
    public Task UpdateAsync(Server.Data.Entities.PlayerInventoryEntity entity) => Task.CompletedTask;
    public Task DeleteAsync(int id) => Task.CompletedTask;
}

public class PlayerInventoryIntegrationTests
{
    [Fact]
    public async Task Add_And_Get_PlayerInventory_Works()
    {
        // Arrange
        var repo = new MockPlayerInventoryRepository();
        var service = new PlayerInventoryService(repo);

        var playerId = Guid.NewGuid();
        var inventoryDto = new PlayerInventoryDto
        {
            PlayerId = playerId,
            Tab = "Main",
            SlotIndex = 0,
            ItemInstanceId = 123,
            Quantity = 1
        };

        // Act
        await service.AddAsync(inventoryDto);
        var all = await service.GetAllAsync();

        // Assert
        var added = Assert.Single(all);
        Assert.Equal(playerId, added.PlayerId);
        Assert.Equal("Main", added.Tab);
        Assert.Equal(0, added.SlotIndex);
        Assert.Equal(123, added.ItemInstanceId);
        Assert.Equal(1, added.Quantity);
    }
}
