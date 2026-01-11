using System;
using Server.Data.Providers;
using Xunit;
using Server.Data.Entities;

public class PlayerEntityTests
{
    [Fact]
    public void PlayerEntity_Property_Set_Get_Works()
    {
        var guid = Guid.NewGuid();
        var player = new PlayerEntity { Id = guid, Nickname = "TestPlayer" };
        Assert.Equal(guid, player.Id);
        Assert.Equal("TestPlayer", player.Nickname);
    }

    [Fact]
    public void PlayerEntity_Default_Values_Are_ZeroOrNull()
    {
        var player = new PlayerEntity { Nickname = null! };
        Assert.Equal(Guid.Empty, player.Id);
        Assert.Null(player.Nickname);
        Assert.Equal(0, player.X);
        Assert.Equal(0, player.Y);
        Assert.Equal(0, player.Hp);
        Assert.Equal(0, player.Level);
    }

    [Fact]
    public void PlayerEntity_Set_All_Properties()
    {
        var guid = Guid.NewGuid();
        var player = new PlayerEntity
        {
            Id = guid,
            Nickname = "Alice",
            X = 10,
            Y = 20,
            Hp = 100,
            Level = 5
        };
        Assert.Equal(guid, player.Id);
        Assert.Equal("Alice", player.Nickname);
        Assert.Equal(10, player.X);
        Assert.Equal(20, player.Y);
        Assert.Equal(100, player.Hp);
        Assert.Equal(5, player.Level);
    }
}
