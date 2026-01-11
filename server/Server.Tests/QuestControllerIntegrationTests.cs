using System;
using Server.Data.Providers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Server.DTOs;

public class QuestControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public QuestControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Add_And_Get_Quest_Works()
    {
        var playerId = Guid.NewGuid();
        var dto = new QuestDto
        {
            PlayerId = playerId,
            Name = "Test Quest",
            Description = "First quest",
            IsCompleted = false
        };
        // Add
        var postResponse = await _client.PostAsJsonAsync("/api/quest", dto);
        postResponse.EnsureSuccessStatusCode();
        var created = await postResponse.Content.ReadFromJsonAsync<QuestDto>();
        Assert.NotNull(created);
        Assert.NotEqual(0, created.Id); // Id가 0이 아니어야 함
        Assert.Equal(playerId, created.PlayerId);
        Assert.Equal("Test Quest", created.Name);
        // Get by player
        var getResponse = await _client.GetAsync($"/api/quest/player/{playerId}");
        getResponse.EnsureSuccessStatusCode();
        var quests = await getResponse.Content.ReadFromJsonAsync<QuestDto[]>();
        Assert.Contains(quests, q => q.Name == "Test Quest");
    }

    [Fact]
    public async Task Add_Duplicate_Quest_Returns_BadRequest()
    {
        var playerId = Guid.NewGuid();
        var dto = new QuestDto { PlayerId = playerId, Name = "Dup Quest", Description = "Q", IsCompleted = false };
        var resp1 = await _client.PostAsJsonAsync("/api/quest", dto);
        resp1.EnsureSuccessStatusCode();
        var resp2 = await _client.PostAsJsonAsync("/api/quest", dto);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp2.StatusCode);
    }

    [Fact]
    public async Task Update_Quest_Works_And_NotFound_Or_Forbidden()
    {
        var playerId = Guid.NewGuid();
        var dto = new QuestDto { PlayerId = playerId, Name = "Up Quest", Description = "Q", IsCompleted = false };
        var post = await _client.PostAsJsonAsync("/api/quest", dto);
        var created = await post.Content.ReadFromJsonAsync<QuestDto>();
        // 정상 수정
        created.Description = "Updated";
        var putResp = await _client.PutAsJsonAsync($"/api/quest/{created.Id}", created);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, putResp.StatusCode);
        // 없는 퀘스트
        var fake = new QuestDto { Id = 9999, PlayerId = playerId, Name = "Fake", Description = "", IsCompleted = false };
        var notFoundResp = await _client.PutAsJsonAsync("/api/quest/9999", fake);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, notFoundResp.StatusCode);
        // 소유자 불일치
        var forbiddenDto = new QuestDto
        {
            Id = created.Id,
            PlayerId = Guid.NewGuid(), // 다른 플레이어
            Name = created.Name,
            Description = created.Description,
            IsCompleted = created.IsCompleted
        };
        var forbidResp = await _client.PutAsJsonAsync($"/api/quest/{created.Id}", forbiddenDto);
        Assert.Equal(System.Net.HttpStatusCode.Forbidden, forbidResp.StatusCode);
    }

    [Fact]
    public async Task Delete_Quest_Works_And_NotFound_Or_Forbidden()
    {
        var playerId = Guid.NewGuid();
        var dto = new QuestDto { PlayerId = playerId, Name = "Del Quest", Description = "Q", IsCompleted = false };
        var post = await _client.PostAsJsonAsync("/api/quest", dto);
        var created = await post.Content.ReadFromJsonAsync<QuestDto>();
        // 정상 삭제
        var delResp = await _client.DeleteAsync($"/api/quest/{created.Id}/player/{playerId}");
        Assert.Equal(System.Net.HttpStatusCode.NoContent, delResp.StatusCode);
        // 없는 퀘스트
        var notFoundResp = await _client.DeleteAsync($"/api/quest/9999/player/{playerId}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, notFoundResp.StatusCode);
        // 소유자 불일치
        var otherPlayer = Guid.NewGuid();
        var dto2 = new QuestDto { PlayerId = playerId, Name = "Del Quest 2", Description = "Q", IsCompleted = false };
        var post2 = await _client.PostAsJsonAsync("/api/quest", dto2);
        var created2 = await post2.Content.ReadFromJsonAsync<QuestDto>();
        var forbidResp = await _client.DeleteAsync($"/api/quest/{created2.Id}/player/{otherPlayer}");
        Assert.Equal(System.Net.HttpStatusCode.Forbidden, forbidResp.StatusCode);
    }
}
