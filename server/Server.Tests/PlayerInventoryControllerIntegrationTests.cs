using System;
using Server.Data.Providers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Server.DTOs;

public class PlayerInventoryControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PlayerInventoryControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Add_And_Get_PlayerInventory_Via_Api_Works()
    {
        // Arrange
        Console.WriteLine("[TEST] Arrange: Create PlayerInventoryDto");
        var playerId = Guid.NewGuid();
        var dto = new PlayerInventoryDto
        {
            PlayerId = playerId,
            Tab = "Main",
            SlotIndex = 0,
            ItemInstanceId = 123,
            Quantity = 1
        };

        // Act: 인벤토리 추가
        Console.WriteLine("[TEST] Act: POST /api/player-inventories");
        var postResponse = await _client.PostAsJsonAsync("/api/player-inventories", dto);
        Console.WriteLine($"[TEST] POST Status: {postResponse.StatusCode}");
        postResponse.EnsureSuccessStatusCode();

        // Act: 전체 인벤토리 조회
        Console.WriteLine("[TEST] Act: GET /api/player-inventories");
        var getResponse = await _client.GetAsync("/api/player-inventories");
        Console.WriteLine($"[TEST] GET Status: {getResponse.StatusCode}");
        getResponse.EnsureSuccessStatusCode();
        var inventories = await getResponse.Content.ReadFromJsonAsync<PlayerInventoryDto[]>();
        Console.WriteLine($"[TEST] GET Result Count: {inventories?.Length}");

        // Assert
        Console.WriteLine("[TEST] Assert: Check inventory contents");
        Assert.NotNull(inventories);
        var added = Assert.Single(inventories, inv => inv.PlayerId == playerId);
        Assert.Equal("Main", added.Tab);
        Assert.Equal(0, added.SlotIndex);
        Assert.Equal(123, added.ItemInstanceId);
        Assert.Equal(1, added.Quantity);
        Console.WriteLine("[TEST] Assert: Success, inventory item matches expected values");
    }
}
