using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
// 필요한 네임스페이스들을 동적으로 추가하거나 미리 정의
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Details;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using Server.Infrastructure.Entities.Refs;
using Server.Infrastructure.Entities.UserContents; // UserContent 네임스페이스 예시
using Server.Enums; // Enum 네임스페이스

namespace Server.Infrastructure.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext() {} // 마이그레이션용 기본 생성자
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    // DbSet 속성들
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<AchievementCategoryEntity> AchievementCategories { get; set; }
    public DbSet<AchievementDefinitionEntity> AchievementDefinitions { get; set; }
    public DbSet<BoardGameDefinitionEntity> BoardGameDefinitions { get; set; }
    public DbSet<BuffDefinitionEntity> BuffDefinitions { get; set; }
    public DbSet<BuildingDefinitionEntity> BuildingDefinitions { get; set; }
    public DbSet<BuildingUpgradeDefinitionEntity> BuildingUpgradeDefinitions { get; set; }
    public DbSet<CategoryRankingEntity> CategoryRankings { get; set; }
    public DbSet<ChannelDefinitionEntity> ChannelDefinitions { get; set; }
    public DbSet<ClassDefinitionEntity> ClassDefinitions { get; set; }
    public DbSet<ClassSkillDefinitionEntity> ClassSkillDefinitions { get; set; }
    public DbSet<ClassTraitDefinitionEntity> ClassTraitDefinitions { get; set; }
    public DbSet<ContainerEntity> Containers { get; set; }
    public DbSet<CookingGradeRewardDefinitionEntity> CookingGradeRewardDefinitions { get; set; }
    public DbSet<CookingRecipeDefinitionEntity> CookingRecipeDefinitions { get; set; }
    public DbSet<CookingResultDefinitionEntity> CookingResultDefinitions { get; set; }
    public DbSet<CookingStepDefinitionEntity> CookingStepDefinitions { get; set; }
    public DbSet<CropDefinitionEntity> CropDefinitions { get; set; }
    public DbSet<CurrencyDefinitionEntity> CurrencyDefinitions { get; set; }
    public DbSet<DialogueDefinitionEntity> DialogueDefinitions { get; set; }
    public DbSet<DropTableDefinitionEntity> DropTableDefinitions { get; set; }
    public DbSet<DungeonDefinitionEntity> DungeonDefinitions { get; set; }
    public DbSet<EntityDefinitionEntity> EntityDefinitions { get; set; }
    public DbSet<EventDefinitionEntity> EventDefinitions { get; set; }
    public DbSet<FarmPlotDefinitionEntity> FarmPlotDefinitions { get; set; }
    public DbSet<FishDefinitionEntity> FishDefinitions { get; set; }
    public DbSet<FishingSpotDefinitionEntity> FishingSpotDefinitions { get; set; }
    public DbSet<GmRoleEntity> GmRoles { get; set; }
    public DbSet<GuildDefinitionEntity> GuildDefinitions { get; set; }
    public DbSet<GuildLevelDefinitionEntity> GuildLevelDefinitions { get; set; }
    public DbSet<ItemDefinitionEntity> ItemDefinitions { get; set; }
    public DbSet<ItemEffectDefinitionEntity> ItemEffectDefinitions { get; set; }
    public DbSet<JobDefinitionEntity> JobDefinitions { get; set; }
    public DbSet<JobLevelDefinitionEntity> JobLevelDefinitions { get; set; }
    public DbSet<JobSkillDefinitionEntity> JobSkillDefinitions { get; set; }
    public DbSet<JobSkillLevelDefinitionEntity> JobSkillLevelDefinitions { get; set; }
    public DbSet<LevelDefinitionEntity> LevelDefinitions { get; set; }
    public DbSet<LivestockDefinitionEntity> LivestockDefinitions { get; set; }
    public DbSet<LogDefinitionEntity> LogDefinitions { get; set; }
    public DbSet<MailEntity> Mails { get; set; }
    public DbSet<MapDefinitionEntity> MapDefinitions { get; set; }
    public DbSet<MiniGameDefinitionEntity> MiniGameDefinitions { get; set; }
    public DbSet<MiniGameRewardDefinitionEntity> MiniGameRewardDefinitions { get; set; }
    public DbSet<MonsterAiBtDefinitionEntity> MonsterAiBtDefinitions { get; set; }
    public DbSet<MonsterAiFsmDefinitionEntity> MonsterAiFsmDefinitions { get; set; }
    public DbSet<MonsterDefinitionEntity> MonsterDefinitions { get; set; }
    public DbSet<NotificationScheduleEntity> NotificationSchedules { get; set; }
    public DbSet<NpcDefinitionEntity> NpcDefinitions { get; set; }
    public DbSet<OreDefinitionEntity> OreDefinitions { get; set; }
    public DbSet<PartyDefinitionEntity> PartyDefinitions { get; set; }
    public DbSet<PetDefinitionEntity> PetDefinitions { get; set; }
    public DbSet<PlayerEntity> Players { get; set; }
    public DbSet<QuestDefinitionEntity> QuestDefinitions { get; set; }
    public DbSet<QuestGroupEntity> QuestGroups { get; set; }
    public DbSet<RecipeDefinitionEntity> RecipeDefinitions { get; set; }
    public DbSet<SeasonPassDefinitionEntity> SeasonPassDefinitions { get; set; }
    public DbSet<ShopDefinitionEntity> ShopDefinitions { get; set; }
    public DbSet<ShopItemDefinitionEntity> ShopItemDefinitions { get; set; }
    public DbSet<SkillDefinitionEntity> SkillDefinitions { get; set; }
    public DbSet<SkillEffectDefinitionEntity> SkillEffectDefinitions { get; set; }
    public DbSet<StatDefinitionEntity> StatDefinitions { get; set; }
    public DbSet<TagEntity> Tags { get; set; }
    public DbSet<TitleCategoryEntity> TitleCategories { get; set; }
    public DbSet<TitleDefinitionEntity> TitleDefinitions { get; set; }
    public DbSet<UserPatternBoardEntity> UserPatternBoards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fluent API 설정
        modelBuilder.Entity<AccountEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<AccountEntity>().HasIndex(e => e.Nickname).IsUnique();
        modelBuilder.Entity<AccountEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<AccountEntity>().Property(e => e.Status).HasDefaultValue("'Active'");
        modelBuilder.Entity<AuthEntity>().Property(e => e.Provider).HasConversion<string>();
        modelBuilder.Entity<AuthEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<AuthEntity>().Property(e => e.IsVerified).HasDefaultValue(false);
        modelBuilder.Entity<PlayerEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<PlayerEntity>().HasIndex(e => e.Nickname).IsUnique();
        modelBuilder.Entity<PlayerEntity>().Property(e => e.Level).HasDefaultValue(1);
        modelBuilder.Entity<PlayerEntity>().Property(e => e.Experience).HasDefaultValue(0);
        modelBuilder.Entity<PlayerEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerEntity>().Property(e => e.PlayTimeSeconds).HasDefaultValue(0);
        modelBuilder.Entity<MapDefinitionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<MapDefinitionEntity>().Property(e => e.ViewMode).HasConversion<string>();
        modelBuilder.Entity<MapDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MapDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<MapDefinitionEntity>().Property(e => e.IsSafeZone).HasDefaultValue(false);
        modelBuilder.Entity<MapEnvironmentEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MapEnvironmentEntity>().HasIndex(e => e.MapId).IsUnique();
        modelBuilder.Entity<DropTableDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<DropTableDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<DropTableDetailEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<DropTableDetailEntity>().Property(e => e.DropRate).HasDefaultValue(1.0);
        modelBuilder.Entity<DropTableDetailEntity>().Property(e => e.MinCount).HasDefaultValue(1);
        modelBuilder.Entity<DropTableDetailEntity>().Property(e => e.MaxCount).HasDefaultValue(1);
        modelBuilder.Entity<EntityDefinitionEntity>().Property(e => e.EntityType).HasConversion<string>();
        modelBuilder.Entity<EntityDefinitionEntity>().Property(e => e.ResourceType).HasConversion<string>();
        modelBuilder.Entity<EntityDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<EntityDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<StatValueEntity>().Property(e => e.OwnerType).HasConversion<string>();
        modelBuilder.Entity<StatValueEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<StatDefinitionEntity>().Property(e => e.StatType).HasConversion<string>();
        modelBuilder.Entity<StatDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<StatDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<StatDefinitionEntity>().Property(e => e.IsPercentage).HasDefaultValue(false);
        modelBuilder.Entity<StatDefinitionEntity>().Property(e => e.DefaultValue).HasDefaultValue(0);
        modelBuilder.Entity<MapObjectInstanceEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MapObjectInstanceEntity>().Property(e => e.RotationX).HasDefaultValue(0);
        modelBuilder.Entity<MapObjectInstanceEntity>().Property(e => e.RotationY).HasDefaultValue(0);
        modelBuilder.Entity<MapObjectInstanceEntity>().Property(e => e.RotationZ).HasDefaultValue(0);
        modelBuilder.Entity<MapObjectInstanceEntity>().Property(e => e.ScaleX).HasDefaultValue(1);
        modelBuilder.Entity<MapObjectInstanceEntity>().Property(e => e.ScaleY).HasDefaultValue(1);
        modelBuilder.Entity<MapObjectInstanceEntity>().Property(e => e.ScaleZ).HasDefaultValue(1);
        modelBuilder.Entity<MapObjectInstanceEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<MapSpawnPointEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MapSpawnPointEntity>().Property(e => e.SpawnRadius).HasDefaultValue(0);
        modelBuilder.Entity<MapSpawnPointEntity>().Property(e => e.MaxConcurrent).HasDefaultValue(1);
        modelBuilder.Entity<MapLayerEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<MapLayerEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MapLayerEntity>().Property(e => e.IsVisible).HasDefaultValue(true);
        modelBuilder.Entity<MapTileEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MapZoneEntity>().Property(e => e.ZoneType).HasConversion<string>();
        modelBuilder.Entity<MapZoneEntity>().Property(e => e.AreaType).HasConversion<string>();
        modelBuilder.Entity<MapZoneEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MapZoneEntity>().Property(e => e.Priority).HasDefaultValue(0);
        modelBuilder.Entity<MapZoneEntity>().Property(e => e.Active).HasDefaultValue(true);
        modelBuilder.Entity<ZoneEventTriggerEntity>().Property(e => e.EventType).HasConversion<string>();
        modelBuilder.Entity<ZoneEventTriggerEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ZoneEventTriggerEntity>().Property(e => e.Priority).HasDefaultValue(0);
        modelBuilder.Entity<MapEventTriggerEntity>().Property(e => e.EventType).HasConversion<string>();
        modelBuilder.Entity<MapEventTriggerEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<EditableAreaEntity>().Property(e => e.AreaType).HasConversion<string>();
        modelBuilder.Entity<EditableAreaEntity>().Property(e => e.OwnerType).HasConversion<string>();
        modelBuilder.Entity<EditableAreaEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<EditableAreaEntity>().Property(e => e.AreaType).HasDefaultValue(MapZoneAreaType.Rect);
        modelBuilder.Entity<EditableAreaEntity>().Property(e => e.IsBuildable).HasDefaultValue(true);
        modelBuilder.Entity<EditableAreaEntity>().Property(e => e.IsFarmable).HasDefaultValue(false);
        modelBuilder.Entity<EditableAreaEntity>().Property(e => e.OwnerType).HasDefaultValue(LandOwnershipType.System);
        modelBuilder.Entity<MapDynamicObjectStateEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MapDynamicObjectStateEntity>().Property(e => e.LastChangedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<MapPlayerStateEntity>().Property(e => e.LastSyncAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.SubType).HasConversion<string>();
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.Rarity).HasConversion<string>();
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.EquipSlot).HasConversion<string>();
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ItemDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.Grade).HasDefaultValue(ItemGrade.Normal);
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.Rarity).HasDefaultValue(ItemRarity.Common);
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.MaxStack).HasDefaultValue(1);
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.IsTradable).HasDefaultValue(true);
        modelBuilder.Entity<ItemDefinitionEntity>().Property(e => e.IsUsable).HasDefaultValue(false);
        modelBuilder.Entity<TagEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<TagEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<ItemEffectDefinitionEntity>().Property(e => e.EffectType).HasConversion<string>();
        modelBuilder.Entity<ItemEffectDefinitionEntity>().Property(e => e.TargetType).HasConversion<string>();
        modelBuilder.Entity<ItemEffectDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ItemEffectDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<ItemEffectDefinitionEntity>().Property(e => e.TargetType).HasDefaultValue(SkillTargetType.Self);
        modelBuilder.Entity<ItemInstanceEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ItemInstanceEntity>().Property(e => e.Quantity).HasDefaultValue(1);
        modelBuilder.Entity<ItemInstanceEntity>().Property(e => e.EnhancementLevel).HasDefaultValue(0);
        modelBuilder.Entity<ItemInstanceEntity>().Property(e => e.AcquiredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<ItemInstanceEntity>().Property(e => e.IsLocked).HasDefaultValue(false);
        modelBuilder.Entity<ContainerEntity>().Property(e => e.ContainerType).HasConversion<string>();
        modelBuilder.Entity<ContainerEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ItemAcquisitionLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ItemAcquisitionLogEntity>().Property(e => e.AcquiredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<ItemEnhancementLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ItemEnhancementLogEntity>().Property(e => e.EnhancedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<RecipeDefinitionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<RecipeDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<RecipeDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<RecipeDefinitionEntity>().Property(e => e.RequiredLevel).HasDefaultValue(1);
        modelBuilder.Entity<RecipeDefinitionEntity>().Property(e => e.SuccessRate).HasDefaultValue(1.0);
        modelBuilder.Entity<RecipeDefinitionEntity>().Property(e => e.CraftTimeSeconds).HasDefaultValue(0);
        modelBuilder.Entity<RecipeDefinitionEntity>().Property(e => e.IsRepeatable).HasDefaultValue(true);
        modelBuilder.Entity<RecipeIngredientEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<RecipeIngredientEntity>().Property(e => e.Consumed).HasDefaultValue(true);
        modelBuilder.Entity<RecipeResultEntity>().Property(e => e.ResultType).HasConversion<string>();
        modelBuilder.Entity<RecipeResultEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<RecipeResultEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<RecipeResultEntity>().Property(e => e.ResultType).HasDefaultValue(RecipeResultType.SuccessNormal);
        modelBuilder.Entity<RecipeResultEntity>().Property(e => e.MinQuantity).HasDefaultValue(1);
        modelBuilder.Entity<RecipeResultEntity>().Property(e => e.MaxQuantity).HasDefaultValue(1);
        modelBuilder.Entity<RecipeResultEntity>().Property(e => e.Probability).HasDefaultValue(1.0);
        modelBuilder.Entity<RecipeResultEntity>().Property(e => e.IsFailReward).HasDefaultValue(false);
        modelBuilder.Entity<PlayerCraftingLogEntity>().Property(e => e.ResultType).HasConversion<string>();
        modelBuilder.Entity<PlayerCraftingLogEntity>().Property(e => e.ResultGrade).HasConversion<string>();
        modelBuilder.Entity<PlayerCraftingLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerCraftingLogEntity>().Property(e => e.CraftedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PortalEntity>().Property(e => e.PortalType).HasConversion<string>();
        modelBuilder.Entity<PortalEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PortalEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<PortalEntity>().Property(e => e.FromRadius).HasDefaultValue(1.0);
        modelBuilder.Entity<PortalEntity>().Property(e => e.PortalType).HasDefaultValue(PortalType.Normal);
        modelBuilder.Entity<PortalEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<PortalEntity>().Property(e => e.IsBidirectional).HasDefaultValue(false);
        modelBuilder.Entity<PortalEntity>().Property(e => e.RequiredItemConsumed).HasDefaultValue(false);
        modelBuilder.Entity<PortalUseLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PortalUseLogEntity>().Property(e => e.UsedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<CropDefinitionEntity>().Property(e => e.AllowedSeason).HasConversion<string>();
        modelBuilder.Entity<CropDefinitionEntity>().Property(e => e.BaseGrade).HasConversion<string>();
        modelBuilder.Entity<CropDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CropDefinitionEntity>().HasIndex(e => e.EntityDefinitionId).IsUnique();
        modelBuilder.Entity<CropDefinitionEntity>().Property(e => e.RequiredFarmingLevel).HasDefaultValue(1);
        modelBuilder.Entity<CropDefinitionEntity>().Property(e => e.BaseGrade).HasDefaultValue(CropGrade.Normal);
        modelBuilder.Entity<CropDefinitionEntity>().Property(e => e.DiseaseChance).HasDefaultValue(0);
        modelBuilder.Entity<CropDefinitionEntity>().Property(e => e.RequiredNutrient).HasDefaultValue(0);
        modelBuilder.Entity<CropDefinitionEntity>().Property(e => e.RequiredMoisture).HasDefaultValue(0);
        modelBuilder.Entity<SeedGradeToCropGradeMapEntity>().Property(e => e.SeedGrade).HasConversion<string>();
        modelBuilder.Entity<SeedGradeToCropGradeMapEntity>().Property(e => e.CropGrade).HasConversion<string>();
        modelBuilder.Entity<SeedGradeToCropGradeMapEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CropProductEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CropProductEntity>().Property(e => e.MinYield).HasDefaultValue(1);
        modelBuilder.Entity<CropProductEntity>().Property(e => e.MaxYield).HasDefaultValue(1);
        modelBuilder.Entity<PlayerCropInstanceEntity>().Property(e => e.CurrentGrade).HasConversion<string>();
        modelBuilder.Entity<PlayerCropInstanceEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerCropInstanceEntity>().Property(e => e.PlantTime).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerCropInstanceEntity>().Property(e => e.GrowthStage).HasDefaultValue(0);
        modelBuilder.Entity<PlayerCropInstanceEntity>().Property(e => e.CurrentGrowthTimeMinutes).HasDefaultValue(0);
        modelBuilder.Entity<PlayerCropInstanceEntity>().Property(e => e.IsWatered).HasDefaultValue(false);
        modelBuilder.Entity<PlayerCropHarvestLogEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<PlayerCropHarvestLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerCropHarvestLogEntity>().Property(e => e.HarvestedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<FarmPlotDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<FarmPlotDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<FishDefinitionEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<FishDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<FishDefinitionEntity>().HasIndex(e => e.EntityDefinitionId).IsUnique();
        modelBuilder.Entity<FishDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<FishDefinitionEntity>().Property(e => e.RequiredFishingLevel).HasDefaultValue(1);
        modelBuilder.Entity<FishingSpotDefinitionEntity>().Property(e => e.AreaType).HasConversion<string>();
        modelBuilder.Entity<FishingSpotDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<FishingSpotDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<FishingSpotDefinitionEntity>().Property(e => e.RequiredFishingLevel).HasDefaultValue(1);
        modelBuilder.Entity<FishHabitatEntity>().Property(e => e.RequiredTime).HasConversion<string>();
        modelBuilder.Entity<FishHabitatEntity>().Property(e => e.RequiredSeason).HasConversion<string>();
        modelBuilder.Entity<FishHabitatEntity>().Property(e => e.RequiredWeather).HasConversion<string>();
        modelBuilder.Entity<FishHabitatEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerFishingLogEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<PlayerFishingLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerFishingLogEntity>().Property(e => e.CaughtAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerFishingLogEntity>().Property(e => e.Success).HasDefaultValue(true);
        modelBuilder.Entity<LivestockDefinitionEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<LivestockDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<LivestockDefinitionEntity>().HasIndex(e => e.EntityDefinitionId).IsUnique();
        modelBuilder.Entity<LivestockDefinitionEntity>().Property(e => e.GrowthStages).HasDefaultValue(3);
        modelBuilder.Entity<LivestockDefinitionEntity>().Property(e => e.RequiredNutritionPerDay).HasDefaultValue(1);
        modelBuilder.Entity<LivestockDefinitionEntity>().Property(e => e.DiseaseChance).HasDefaultValue(0);
        modelBuilder.Entity<LivestockDefinitionEntity>().Property(e => e.MinProductAmount).HasDefaultValue(1);
        modelBuilder.Entity<LivestockDefinitionEntity>().Property(e => e.MaxProductAmount).HasDefaultValue(1);
        modelBuilder.Entity<PlayerLivestockInstanceEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<PlayerLivestockInstanceEntity>().Property(e => e.CurrentGrowthStage).HasConversion<string>();
        modelBuilder.Entity<PlayerLivestockInstanceEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerLivestockInstanceEntity>().Property(e => e.AcquiredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerLivestockInstanceEntity>().Property(e => e.CurrentGrowthStage).HasDefaultValue(LivestockGrowthStage.Baby);
        modelBuilder.Entity<PlayerLivestockInstanceEntity>().Property(e => e.CurrentGrowthTimeMinutes).HasDefaultValue(0);
        modelBuilder.Entity<PlayerLivestockInstanceEntity>().Property(e => e.CurrentNutrition).HasDefaultValue(0);
        modelBuilder.Entity<PlayerLivestockProductLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerLivestockProductLogEntity>().Property(e => e.ProducedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerLivestockFeedLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerLivestockFeedLogEntity>().Property(e => e.FedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerLivestockDiseaseLogEntity>().Property(e => e.EventType).HasConversion<string>();
        modelBuilder.Entity<PlayerLivestockDiseaseLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerLivestockDiseaseLogEntity>().Property(e => e.OccurredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerLivestockGradeLogEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<PlayerLivestockGradeLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerLivestockGradeLogEntity>().Property(e => e.EvaluatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerLivestockGrowthLogEntity>().Property(e => e.Stage).HasConversion<string>();
        modelBuilder.Entity<PlayerLivestockGrowthLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerLivestockGrowthLogEntity>().Property(e => e.ChangedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<MonsterDefinitionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<MonsterDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MonsterDefinitionEntity>().HasIndex(e => e.EntityDefinitionId).IsUnique();
        modelBuilder.Entity<MonsterDefinitionEntity>().Property(e => e.Type).HasDefaultValue(MonsterType.Normal);
        modelBuilder.Entity<MonsterDefinitionEntity>().Property(e => e.ExpReward).HasDefaultValue(0);
        modelBuilder.Entity<MonsterAiFsmDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MonsterAiFsmDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<MonsterAiBtDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MonsterAiBtDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<PlayerMonsterKillLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerMonsterKillLogEntity>().Property(e => e.KilledAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<OreDefinitionEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<OreDefinitionEntity>().Property(e => e.RequiredToolSubtype).HasConversion<string>();
        modelBuilder.Entity<OreDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<OreDefinitionEntity>().HasIndex(e => e.EntityDefinitionId).IsUnique();
        modelBuilder.Entity<OreDefinitionEntity>().Property(e => e.RequiredToolLevel).HasDefaultValue(1);
        modelBuilder.Entity<OreDefinitionEntity>().Property(e => e.RequiredSkillLevel).HasDefaultValue(1);
        modelBuilder.Entity<OreDefinitionEntity>().Property(e => e.MiningDifficulty).HasDefaultValue(1);
        modelBuilder.Entity<LogDefinitionEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<LogDefinitionEntity>().Property(e => e.RequiredToolSubtype).HasConversion<string>();
        modelBuilder.Entity<LogDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<LogDefinitionEntity>().HasIndex(e => e.EntityDefinitionId).IsUnique();
        modelBuilder.Entity<LogDefinitionEntity>().Property(e => e.RequiredToolLevel).HasDefaultValue(1);
        modelBuilder.Entity<LogDefinitionEntity>().Property(e => e.RequiredSkillLevel).HasDefaultValue(1);
        modelBuilder.Entity<LogDefinitionEntity>().Property(e => e.LoggingDifficulty).HasDefaultValue(1);
        modelBuilder.Entity<PlayerMiningLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerMiningLogEntity>().Property(e => e.MinedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerLoggingLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerLoggingLogEntity>().Property(e => e.LoggedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<LevelDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<LevelDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<PlayerLevelLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerLevelLogEntity>().Property(e => e.ChangedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerStatLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerStatLogEntity>().Property(e => e.ChangedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerRecipeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerRecipeEntity>().Property(e => e.UnlockedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerRecipeEntity>().Property(e => e.IsKnown).HasDefaultValue(true);
        modelBuilder.Entity<PlayerRecipeEntity>().Property(e => e.SuccessCount).HasDefaultValue(0);
        modelBuilder.Entity<PlayerRecipeEntity>().Property(e => e.FailCount).HasDefaultValue(0);
        modelBuilder.Entity<PlayerRecipeEntity>().Property(e => e.PityCounter).HasDefaultValue(0);
        modelBuilder.Entity<MiniGameDefinitionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<MiniGameDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MiniGameDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<MiniGameRewardDefinitionEntity>().Property(e => e.ResultGrade).HasConversion<string>();
        modelBuilder.Entity<MiniGameRewardDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MiniGameRewardDefinitionEntity>().Property(e => e.MinQuantity).HasDefaultValue(1);
        modelBuilder.Entity<MiniGameRewardDefinitionEntity>().Property(e => e.MaxQuantity).HasDefaultValue(1);
        modelBuilder.Entity<MiniGameRewardDefinitionEntity>().Property(e => e.Probability).HasDefaultValue(1.0);
        modelBuilder.Entity<PlayerMiniGameResultEntity>().Property(e => e.ResultGrade).HasConversion<string>();
        modelBuilder.Entity<PlayerMiniGameResultEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerMiniGameResultEntity>().Property(e => e.PlayedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<CookingRecipeDefinitionEntity>().Property(e => e.OptimalDoneness).HasConversion<string>();
        modelBuilder.Entity<CookingRecipeDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CookingRecipeDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<CookingRecipeDefinitionEntity>().HasIndex(e => e.RecipeDefinitionId).IsUnique();
        modelBuilder.Entity<CookingResultDefinitionEntity>().Property(e => e.Doneness).HasConversion<string>();
        modelBuilder.Entity<CookingResultDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CookingResultDefinitionEntity>().Property(e => e.Probability).HasDefaultValue(1.0);
        modelBuilder.Entity<CookingGradeRewardDefinitionEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<CookingGradeRewardDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CookingGradeRewardDefinitionEntity>().Property(e => e.Probability).HasDefaultValue(1.0);
        modelBuilder.Entity<CookingStepDefinitionEntity>().Property(e => e.Action).HasConversion<string>();
        modelBuilder.Entity<CookingStepDefinitionEntity>().Property(e => e.RequiredTool).HasConversion<string>();
        modelBuilder.Entity<CookingStepDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerCookingLogEntity>().Property(e => e.Grade).HasConversion<string>();
        modelBuilder.Entity<PlayerCookingLogEntity>().Property(e => e.Doneness).HasConversion<string>();
        modelBuilder.Entity<PlayerCookingLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerCookingLogEntity>().Property(e => e.CookedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<JobDefinitionEntity>().Property(e => e.JobType).HasConversion<string>();
        modelBuilder.Entity<JobDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<JobDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<JobDefinitionEntity>().Property(e => e.IsPlayable).HasDefaultValue(true);
        modelBuilder.Entity<JobDefinitionEntity>().Property(e => e.IsHidden).HasDefaultValue(false);
        modelBuilder.Entity<JobDefinitionEntity>().Property(e => e.OrderIndex).HasDefaultValue(0);
        modelBuilder.Entity<JobSkillDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<JobSkillDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<JobSkillDefinitionEntity>().Property(e => e.MaxLevel).HasDefaultValue(1);
        modelBuilder.Entity<PlayerJobSkillEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerJobSkillEntity>().Property(e => e.Level).HasDefaultValue(0);
        modelBuilder.Entity<PlayerJobSkillEntity>().Property(e => e.AcquiredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<JobSkillLevelDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<JobSkillLevelDefinitionEntity>().Property(e => e.RequiredSkillPoints).HasDefaultValue(1);
        modelBuilder.Entity<PlayerJobSkillPointEntity>().Property(e => e.TotalPoints).HasDefaultValue(0);
        modelBuilder.Entity<PlayerJobSkillPointEntity>().Property(e => e.UsedPoints).HasDefaultValue(0);
        modelBuilder.Entity<PlayerJobSkillPointEntity>().Property(e => e.LastUpdatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<JobLevelDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<JobLevelDefinitionEntity>().Property(e => e.RewardSkillPoints).HasDefaultValue(0);
        modelBuilder.Entity<JobSkillTreeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<JobTreeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SkillDefinitionEntity>().Property(e => e.TargetType).HasConversion<string>();
        modelBuilder.Entity<SkillDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SkillDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<SkillDefinitionEntity>().Property(e => e.MaxLevel).HasDefaultValue(1);
        modelBuilder.Entity<SkillEffectDefinitionEntity>().Property(e => e.EffectType).HasConversion<string>();
        modelBuilder.Entity<SkillEffectDefinitionEntity>().Property(e => e.TargetType).HasConversion<string>();
        modelBuilder.Entity<SkillEffectDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SkillEffectDefinitionEntity>().Property(e => e.Level).HasDefaultValue(1);
        modelBuilder.Entity<SkillTreeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerSkillEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerSkillEntity>().Property(e => e.Level).HasDefaultValue(0);
        modelBuilder.Entity<PlayerSkillEntity>().Property(e => e.IsLearned).HasDefaultValue(false);
        modelBuilder.Entity<PlayerSkillLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerSkillLogEntity>().Property(e => e.LogTime).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerQuickSlotPresetEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerQuickSlotPresetEntity>().Property(e => e.IsActive).HasDefaultValue(false);
        modelBuilder.Entity<PlayerQuickSlotPresetEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerQuickSlotEntity>().Property(e => e.SlotType).HasConversion<string>();
        modelBuilder.Entity<PlayerQuickSlotEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ClassDefinitionEntity>().Property(e => e.Role).HasConversion<string>();
        modelBuilder.Entity<ClassDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ClassDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<ClassDefinitionEntity>().Property(e => e.IsBase).HasDefaultValue(false);
        modelBuilder.Entity<ClassDefinitionEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<ClassTreeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerClassEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerClassEntity>().Property(e => e.IsMain).HasDefaultValue(false);
        modelBuilder.Entity<PlayerClassEntity>().Property(e => e.IsUnlocked).HasDefaultValue(false);
        modelBuilder.Entity<PlayerClassEntity>().Property(e => e.Level).HasDefaultValue(1);
        modelBuilder.Entity<PlayerClassEntity>().Property(e => e.Exp).HasDefaultValue(0);
        modelBuilder.Entity<ClassEquipmentRestrictionEntity>().Property(e => e.ItemType).HasConversion<string>();
        modelBuilder.Entity<ClassEquipmentRestrictionEntity>().Property(e => e.ItemSubType).HasConversion<string>();
        modelBuilder.Entity<ClassEquipmentRestrictionEntity>().Property(e => e.RestrictionType).HasConversion<string>();
        modelBuilder.Entity<ClassEquipmentRestrictionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ClassTraitDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ClassSkillDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ClassSkillDefinitionEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<ClassSkillTreeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerClassChangeLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerClassChangeLogEntity>().Property(e => e.ChangedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerSkillChangeLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerSkillChangeLogEntity>().Property(e => e.ChangedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<ChannelDefinitionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<ChannelDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<ChannelDefinitionEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<ChatLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ChatLogEntity>().Property(e => e.SentAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<ChatLogEntity>().Property(e => e.IsReported).HasDefaultValue(false);
        modelBuilder.Entity<ChatLogEntity>().Property(e => e.IsDeleted).HasDefaultValue(false);
        modelBuilder.Entity<CommunityReportLogEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<CommunityReportLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CommunityReportLogEntity>().Property(e => e.Status).HasDefaultValue(ReportStatus.Pending);
        modelBuilder.Entity<CommunityReportLogEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<ModerationActionLogEntity>().Property(e => e.ActionType).HasConversion<string>();
        modelBuilder.Entity<ModerationActionLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ModerationActionLogEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<FriendRelationEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<FriendRelationEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PartyDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PartyDefinitionEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PartyMemberEntity>().Property(e => e.Role).HasConversion<string>();
        modelBuilder.Entity<PartyMemberEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PartyMemberEntity>().Property(e => e.JoinedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PartyMemberEntity>().Property(e => e.Role).HasDefaultValue(PartyRole.Member);
        modelBuilder.Entity<PartyMemberEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<CommunityNoticeEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<CommunityNoticeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CommunityNoticeEntity>().Property(e => e.Type).HasDefaultValue(NoticeType.General);
        modelBuilder.Entity<CommunityNoticeEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<CommunityNoticeEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<GuildDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GuildDefinitionEntity>().HasIndex(e => e.Name).IsUnique();
        modelBuilder.Entity<GuildDefinitionEntity>().HasIndex(e => e.Tag).IsUnique();
        modelBuilder.Entity<GuildDefinitionEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<GuildDefinitionEntity>().Property(e => e.Level).HasDefaultValue(1);
        modelBuilder.Entity<GuildDefinitionEntity>().Property(e => e.Experience).HasDefaultValue(0);
        modelBuilder.Entity<GuildDefinitionEntity>().Property(e => e.MemberLimit).HasDefaultValue(20);
        modelBuilder.Entity<GuildDefinitionEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<GuildDefinitionEntity>().Property(e => e.IsRecruiting).HasDefaultValue(false);
        modelBuilder.Entity<GuildRoleEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GuildRoleEntity>().Property(e => e.IsLeaderRole).HasDefaultValue(false);
        modelBuilder.Entity<GuildRoleEntity>().Property(e => e.RoleOrder).HasDefaultValue(0);
        modelBuilder.Entity<GuildMemberEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GuildMemberEntity>().Property(e => e.JoinedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<GuildMemberEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<GuildMemberEntity>().Property(e => e.ContributionPoints).HasDefaultValue(0);
        modelBuilder.Entity<GuildNoticeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GuildNoticeEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<GuildNoticeEntity>().Property(e => e.IsPinned).HasDefaultValue(false);
        modelBuilder.Entity<GuildNoticeEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<GuildJoinRequestEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<GuildJoinRequestEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GuildJoinRequestEntity>().Property(e => e.Status).HasDefaultValue(GuildJoinRequestStatus.Pending);
        modelBuilder.Entity<GuildJoinRequestEntity>().Property(e => e.RequestedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<GuildContributionLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GuildContributionLogEntity>().Property(e => e.LoggedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<CategoryRankingEntity>().Property(e => e.Category).HasConversion<string>();
        modelBuilder.Entity<CategoryRankingEntity>().Property(e => e.TargetType).HasConversion<string>();
        modelBuilder.Entity<CategoryRankingEntity>().Property(e => e.Season).HasConversion<string>();
        modelBuilder.Entity<CategoryRankingEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<RankingHistoryEntity>().Property(e => e.Category).HasConversion<string>();
        modelBuilder.Entity<RankingHistoryEntity>().Property(e => e.TargetType).HasConversion<string>();
        modelBuilder.Entity<RankingHistoryEntity>().Property(e => e.Season).HasConversion<string>();
        modelBuilder.Entity<RankingHistoryEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<RecommendationEntity>().Property(e => e.TargetType).HasConversion<string>();
        modelBuilder.Entity<RecommendationEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<RecommendationEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<RecommendationLogEntity>().Property(e => e.Action).HasConversion<string>();
        modelBuilder.Entity<RecommendationLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<RecommendationLogEntity>().Property(e => e.OccurredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<TitleDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<TitleDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<TitleCategoryEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<TitleCategoryEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<TitleCategoryEntity>().Property(e => e.DisplayOrder).HasDefaultValue(0);
        modelBuilder.Entity<TitleEffectEntity>().Property(e => e.EffectType).HasConversion<string>();
        modelBuilder.Entity<TitleEffectEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<TitleUnlockConditionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<TitleUnlockConditionEntity>().Property(e => e.Operator).HasConversion<string>();
        modelBuilder.Entity<TitleUnlockConditionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<TitleUnlockConditionEntity>().Property(e => e.ConditionGroup).HasDefaultValue(1);
        modelBuilder.Entity<PlayerTitleEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerTitleEntity>().Property(e => e.AcquiredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerTitleEntity>().Property(e => e.IsActive).HasDefaultValue(false);
        modelBuilder.Entity<PlayerTitleSlotEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<TitleUnlockHistoryEntity>().Property(e => e.Event).HasConversion<string>();
        modelBuilder.Entity<TitleUnlockHistoryEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<TitleUnlockHistoryEntity>().Property(e => e.OccurredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<LandPlotEntity>().Property(e => e.PlotType).HasConversion<string>();
        modelBuilder.Entity<LandPlotEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<LandPlotEntity>().Property(e => e.IsPublic).HasDefaultValue(false);
        modelBuilder.Entity<PlayerLandEntity>().Property(e => e.OwnerType).HasConversion<string>();
        modelBuilder.Entity<PlayerLandEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerLandEntity>().Property(e => e.PurchasedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerLandEntity>().Property(e => e.IsPrimary).HasDefaultValue(false);
        modelBuilder.Entity<NotificationEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<NotificationEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<NotificationEntity>().Property(e => e.IsRead).HasDefaultValue(false);
        modelBuilder.Entity<NotificationEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<GuildTitleEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GuildTitleEntity>().Property(e => e.AcquiredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<GuildTitleEntity>().Property(e => e.IsActive).HasDefaultValue(false);
        modelBuilder.Entity<BuffDefinitionEntity>().Property(e => e.Category).HasConversion<string>();
        modelBuilder.Entity<BuffDefinitionEntity>().Property(e => e.EffectType).HasConversion<string>();
        modelBuilder.Entity<BuffDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<BuffDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<BuffDefinitionEntity>().Property(e => e.MaxStack).HasDefaultValue(1);
        modelBuilder.Entity<BuffDefinitionEntity>().Property(e => e.IsDispellable).HasDefaultValue(true);
        modelBuilder.Entity<PlayerBuffEntity>().Property(e => e.SourceType).HasConversion<string>();
        modelBuilder.Entity<PlayerBuffEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerBuffEntity>().Property(e => e.StartedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerBuffEntity>().Property(e => e.StackCount).HasDefaultValue(1);
        modelBuilder.Entity<PlayerBuffLogEntity>().Property(e => e.Action).HasConversion<string>();
        modelBuilder.Entity<PlayerBuffLogEntity>().Property(e => e.SourceType).HasConversion<string>();
        modelBuilder.Entity<PlayerBuffLogEntity>().Property(e => e.TriggerCondition).HasConversion<string>();
        modelBuilder.Entity<PlayerBuffLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerBuffLogEntity>().Property(e => e.OccurredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<SkillGrantedBuffEntity>().Property(e => e.TriggerCondition).HasConversion<string>();
        modelBuilder.Entity<SkillGrantedBuffEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SkillGrantedBuffEntity>().Property(e => e.Chance).HasDefaultValue(1.0);
        modelBuilder.Entity<PetDefinitionEntity>().Property(e => e.Rarity).HasConversion<string>();
        modelBuilder.Entity<PetDefinitionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<PetDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PetDefinitionEntity>().HasIndex(e => e.EntityDefinitionId).IsUnique();
        modelBuilder.Entity<PetDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<PetDefinitionEntity>().Property(e => e.Rarity).HasDefaultValue(PetRarity.Common);
        modelBuilder.Entity<PlayerPetEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerPetEntity>().Property(e => e.Level).HasDefaultValue(1);
        modelBuilder.Entity<PlayerPetEntity>().Property(e => e.Exp).HasDefaultValue(0);
        modelBuilder.Entity<PlayerPetEntity>().Property(e => e.AcquiredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerPetEntity>().Property(e => e.IsSummoned).HasDefaultValue(false);
        modelBuilder.Entity<PlayerPetEntity>().Property(e => e.IsLocked).HasDefaultValue(false);
        modelBuilder.Entity<PetSkillEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PetSkillEntity>().Property(e => e.UnlockLevel).HasDefaultValue(1);
        modelBuilder.Entity<PetSkillEntity>().Property(e => e.IsPassive).HasDefaultValue(false);
        modelBuilder.Entity<PlayerPetSkillEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerPetSkillEntity>().Property(e => e.Level).HasDefaultValue(1);
        modelBuilder.Entity<PlayerPetSkillEntity>().Property(e => e.AcquiredAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerPetSkillEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<MailEntity>().Property(e => e.SenderType).HasConversion<string>();
        modelBuilder.Entity<MailEntity>().Property(e => e.ReceiverType).HasConversion<string>();
        modelBuilder.Entity<MailEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<MailEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MailEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<MailEntity>().Property(e => e.SentAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<MailEntity>().Property(e => e.Status).HasDefaultValue(MailStatus.Unread);
        modelBuilder.Entity<CurrencyDefinitionEntity>().Property(e => e.CurrencyType).HasConversion<string>();
        modelBuilder.Entity<CurrencyDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CurrencyDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<CurrencyDefinitionEntity>().Property(e => e.IsPremium).HasDefaultValue(false);
        modelBuilder.Entity<MailAttachmentEntity>().Property(e => e.AttachmentType).HasConversion<string>();
        modelBuilder.Entity<MailAttachmentEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MailReadLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MailReadLogEntity>().Property(e => e.ReadAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<MailDeleteLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MailDeleteLogEntity>().Property(e => e.DeletedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<EventDefinitionEntity>().Property(e => e.EventType).HasConversion<string>();
        modelBuilder.Entity<EventDefinitionEntity>().Property(e => e.ReceiverType).HasConversion<string>();
        modelBuilder.Entity<EventDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<EventDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<EventDefinitionEntity>().Property(e => e.ReceiverType).HasDefaultValue(EventReceiverType.All);
        modelBuilder.Entity<EventDefinitionEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<PlayerEventProgressEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<EventRewardEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerEventRewardLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerEventRewardLogEntity>().Property(e => e.ClaimedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<SeasonPassDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SeasonPassDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<SeasonPassDefinitionEntity>().Property(e => e.Priority).HasDefaultValue(0);
        modelBuilder.Entity<SeasonPassDefinitionEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<SeasonPassDefinitionEntity>().Property(e => e.FreeTrack).HasDefaultValue(true);
        modelBuilder.Entity<SeasonPassDefinitionEntity>().Property(e => e.PremiumTrack).HasDefaultValue(true);
        modelBuilder.Entity<SeasonPassMissionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<SeasonPassMissionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SeasonPassMissionEntity>().Property(e => e.Repeatable).HasDefaultValue(false);
        modelBuilder.Entity<SeasonPassMissionEntity>().Property(e => e.RewardExp).HasDefaultValue(0);
        modelBuilder.Entity<SeasonPassMissionEntity>().Property(e => e.OrderIndex).HasDefaultValue(0);
        modelBuilder.Entity<PlayerSeasonPassEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerSeasonPassEntity>().Property(e => e.PremiumUnlocked).HasDefaultValue(false);
        modelBuilder.Entity<PlayerSeasonPassEntity>().Property(e => e.CurrentLevel).HasDefaultValue(1);
        modelBuilder.Entity<PlayerSeasonPassEntity>().Property(e => e.CurrentExp).HasDefaultValue(0);
        modelBuilder.Entity<PlayerSeasonPassEntity>().Property(e => e.LastUpdatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerSeasonPassMissionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerSeasonPassMissionEntity>().Property(e => e.Progress).HasDefaultValue(0);
        modelBuilder.Entity<SeasonPassRewardEntity>().Property(e => e.Track).HasConversion<string>();
        modelBuilder.Entity<SeasonPassRewardEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SeasonPassRewardEntity>().Property(e => e.IsHidden).HasDefaultValue(false);
        modelBuilder.Entity<PlayerSeasonPassRewardLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerSeasonPassRewardLogEntity>().Property(e => e.ClaimedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<SeasonPassPurchaseLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SeasonPassPurchaseLogEntity>().Property(e => e.PurchasedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<SeasonPassMissionLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SeasonPassMissionLogEntity>().Property(e => e.LoggedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<NotificationScheduleEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<NotificationScheduleEntity>().Property(e => e.TargetType).HasConversion<string>();
        modelBuilder.Entity<NotificationScheduleEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<NotificationScheduleEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<NotificationScheduleEntity>().Property(e => e.Status).HasDefaultValue(NotificationStatus.Scheduled);
        modelBuilder.Entity<NotificationScheduleEntity>().Property(e => e.RetryCount).HasDefaultValue(0);
        modelBuilder.Entity<NotificationLogEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<NotificationLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<NotificationLogEntity>().Property(e => e.SentAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerNotificationLogEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<PlayerNotificationLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerNotificationLogEntity>().Property(e => e.Status).HasDefaultValue(PlayerNotificationStatus.Pending);
        modelBuilder.Entity<BuildingDefinitionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<BuildingDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<BuildingDefinitionEntity>().HasIndex(e => e.EntityDefinitionId).IsUnique();
        modelBuilder.Entity<BuildingDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<BuildingDefinitionEntity>().Property(e => e.MaxLevel).HasDefaultValue(1);
        modelBuilder.Entity<PlayerBuildingEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<PlayerBuildingEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerBuildingEntity>().Property(e => e.RotationY).HasDefaultValue(0);
        modelBuilder.Entity<PlayerBuildingEntity>().Property(e => e.PlacedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerBuildingEntity>().Property(e => e.Level).HasDefaultValue(1);
        modelBuilder.Entity<PlayerBuildingEntity>().Property(e => e.Status).HasDefaultValue(BuildingStatus.Placed);
        modelBuilder.Entity<BuildingUpgradeDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<BuildingUpgradeDefinitionEntity>().Property(e => e.UpgradeTimeSeconds).HasDefaultValue(0);
        modelBuilder.Entity<BuildingUpgradeEffectEntity>().Property(e => e.EffectType).HasConversion<string>();
        modelBuilder.Entity<BuildingUpgradeEffectEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<BuildingActionLogEntity>().Property(e => e.Action).HasConversion<string>();
        modelBuilder.Entity<BuildingActionLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<BuildingActionLogEntity>().Property(e => e.ActionAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerBuildingSlotEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerInventoryTabLimitEntity>().Property(e => e.InventoryType).HasConversion<string>();
        modelBuilder.Entity<PlayerInventoryTabLimitEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerInventoryItemEntity>().Property(e => e.InventoryType).HasConversion<string>();
        modelBuilder.Entity<PlayerInventoryItemEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<PlayerInventoryItemEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerInventoryItemEntity>().HasIndex(e => e.ItemInstanceId).IsUnique();
        modelBuilder.Entity<PlayerInventoryItemEntity>().Property(e => e.Status).HasDefaultValue(InventoryItemStatus.Normal);
        modelBuilder.Entity<MarketListingEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<MarketListingEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MarketListingEntity>().Property(e => e.Status).HasDefaultValue(MarketListingStatus.Listed);
        modelBuilder.Entity<MarketListingEntity>().Property(e => e.ListedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<MarketTransactionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<MarketTransactionEntity>().Property(e => e.TransactedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<TradeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<TradeEntity>().Property(e => e.Player1Accepted).HasDefaultValue(false);
        modelBuilder.Entity<TradeEntity>().Property(e => e.Player2Accepted).HasDefaultValue(false);
        modelBuilder.Entity<TradeEntity>().Property(e => e.Status).HasDefaultValue("Pending");
        modelBuilder.Entity<TradeEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<TradeItemEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CurrencyTransactionLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<CurrencyTransactionLogEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<BoardGameDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<BoardGameDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<BoardGameDefinitionEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<BoardGameRoomEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<BoardGameRoomEntity>().Property(e => e.Status).HasDefaultValue("Waiting");
        modelBuilder.Entity<BoardGameRoomEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<BoardGameParticipantEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<BoardGameParticipantEntity>().Property(e => e.JoinTime).HasDefaultValueSql("now()");
        modelBuilder.Entity<BoardGameTurnLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<BoardGameTurnLogEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<UserPatternEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<UserPatternEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<UserPatternEntity>().Property(e => e.Status).HasDefaultValue(UserContentStatus.Private);
        modelBuilder.Entity<UserPatternEntity>().Property(e => e.LikeCount).HasDefaultValue(0);
        modelBuilder.Entity<UserPatternEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternEntity>().Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternLikeEntity>().Property(e => e.LikedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternCommentEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<UserPatternCommentEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternCommentEntity>().Property(e => e.IsDeleted).HasDefaultValue(false);
        modelBuilder.Entity<UserRoomEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<UserRoomEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<UserRoomEntity>().Property(e => e.Status).HasDefaultValue(UserContentStatus.Private);
        modelBuilder.Entity<UserRoomEntity>().Property(e => e.MaxPlayers).HasDefaultValue(10);
        modelBuilder.Entity<UserRoomEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserRoomEntity>().Property(e => e.LastUpdatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserRoomEntity>().Property(e => e.LikeCount).HasDefaultValue(0);
        modelBuilder.Entity<UserRoomParticipantEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<UserRoomParticipantEntity>().Property(e => e.Role).HasDefaultValue("Guest");
        modelBuilder.Entity<UserRoomParticipantEntity>().Property(e => e.JoinedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserRoomChatEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<UserRoomChatEntity>().Property(e => e.SentAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserContentReportEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<UserContentReportEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<UserContentReportEntity>().Property(e => e.Status).HasDefaultValue(UgcReportStatus.Pending);
        modelBuilder.Entity<UserContentReportEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserRoomFavoriteEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternFavoriteEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternBoardEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<UserPatternBoardEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<UserPatternBoardEntity>().Property(e => e.IsPublic).HasDefaultValue(true);
        modelBuilder.Entity<UserPatternBoardEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternBoardPostEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<UserPatternBoardPostEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternBoardPostEntity>().Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternBoardPostEntity>().Property(e => e.IsPinned).HasDefaultValue(false);
        modelBuilder.Entity<UserPatternBoardPostEntity>().Property(e => e.IsDeleted).HasDefaultValue(false);
        modelBuilder.Entity<UserPatternBoardPostCommentEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<UserPatternBoardPostCommentEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<UserPatternBoardPostCommentEntity>().Property(e => e.IsDeleted).HasDefaultValue(false);
        modelBuilder.Entity<DungeonDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<DungeonDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<DungeonDefinitionEntity>().Property(e => e.MinPlayers).HasDefaultValue(1);
        modelBuilder.Entity<DungeonDefinitionEntity>().Property(e => e.MaxPlayers).HasDefaultValue(1);
        modelBuilder.Entity<DungeonDefinitionEntity>().Property(e => e.LevelRequirement).HasDefaultValue(1);
        modelBuilder.Entity<DungeonDefinitionEntity>().Property(e => e.IsRandom).HasDefaultValue(false);
        modelBuilder.Entity<DungeonDefinitionEntity>().Property(e => e.HasBoss).HasDefaultValue(false);
        modelBuilder.Entity<DungeonZoneLinkEntity>().Property(e => e.ZoneType).HasConversion<string>();
        modelBuilder.Entity<DungeonZoneLinkEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<DungeonZoneLinkEntity>().Property(e => e.IsStartZone).HasDefaultValue(false);
        modelBuilder.Entity<DungeonZoneLinkEntity>().Property(e => e.IsEndZone).HasDefaultValue(false);
        modelBuilder.Entity<DungeonRunEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<DungeonRunEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<DungeonRunEntity>().Property(e => e.Status).HasDefaultValue(DungeonRunStatus.InProgress);
        modelBuilder.Entity<DungeonRunEntity>().Property(e => e.StartedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<DungeonRunEntity>().Property(e => e.ElapsedTimeSeconds).HasDefaultValue(0);
        modelBuilder.Entity<DungeonRunParticipantEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<DungeonRunParticipantEntity>().Property(e => e.JoinedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<DungeonRunParticipantEntity>().Property(e => e.IsCleared).HasDefaultValue(false);
        modelBuilder.Entity<SecurityIncidentLogEntity>().Property(e => e.IncidentType).HasConversion<string>();
        modelBuilder.Entity<SecurityIncidentLogEntity>().Property(e => e.ActionTaken).HasConversion<string>();
        modelBuilder.Entity<SecurityIncidentLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<SecurityIncidentLogEntity>().Property(e => e.DetectedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<SecurityIncidentLogEntity>().Property(e => e.Severity).HasDefaultValue(1);
        modelBuilder.Entity<SecurityIncidentLogEntity>().Property(e => e.ActionTaken).HasDefaultValue(SecurityActionTaken.None);
        modelBuilder.Entity<PlayerBanEntity>().Property(e => e.BanType).HasConversion<string>();
        modelBuilder.Entity<PlayerBanEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerBanEntity>().Property(e => e.StartAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<GmRoleEntity>().Property(e => e.RoleType).HasConversion<string>();
        modelBuilder.Entity<GmRoleEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GmRoleEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<GmAccountEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GmAccountEntity>().HasIndex(e => e.AccountId).IsUnique();
        modelBuilder.Entity<GmAccountEntity>().HasIndex(e => e.GmName).IsUnique();
        modelBuilder.Entity<GmAccountEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<GmActionLogEntity>().Property(e => e.ActionType).HasConversion<string>();
        modelBuilder.Entity<GmActionLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GmActionLogEntity>().Property(e => e.ActionAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<GmNoticeEntity>().Property(e => e.NoticeType).HasConversion<string>();
        modelBuilder.Entity<GmNoticeEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<GmNoticeEntity>().Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<GmNoticeEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<WorldEventEntity>().Property(e => e.EventType).HasConversion<string>();
        modelBuilder.Entity<WorldEventEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<WorldEventEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<WorldEventEntity>().Property(e => e.IsActive).HasDefaultValue(false);
        modelBuilder.Entity<PlayerWorldEventParticipationEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<PlayerWorldEventParticipationEntity>().Property(e => e.JoinedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<PlayerWorldEventParticipationEntity>().Property(e => e.RewardClaimed).HasDefaultValue(false);
        modelBuilder.Entity<ShopDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ShopDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<ShopItemDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ShopItemDefinitionEntity>().Property(e => e.IsLimitedTime).HasDefaultValue(false);
        modelBuilder.Entity<PlayerShopPurchaseLimitEntity>().Property(e => e.PurchaseCount).HasDefaultValue(0);
        modelBuilder.Entity<PlayerShopPurchaseLimitEntity>().Property(e => e.LastPurchaseAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<ShopTransactionLogEntity>().Property(e => e.TransactionType).HasConversion<string>();
        modelBuilder.Entity<ShopTransactionLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<ShopTransactionLogEntity>().Property(e => e.Timestamp).HasDefaultValueSql("now()");
        modelBuilder.Entity<NpcDefinitionEntity>().Property(e => e.NpcType).HasConversion<string>();
        modelBuilder.Entity<NpcDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<NpcDefinitionEntity>().HasIndex(e => e.EntityDefinitionId).IsUnique();
        modelBuilder.Entity<NpcDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<NpcDefinitionEntity>().Property(e => e.IsQuestGiver).HasDefaultValue(false);
        modelBuilder.Entity<NpcInstanceEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<NpcInstanceEntity>().Property(e => e.RotationY).HasDefaultValue(0);
        modelBuilder.Entity<NpcInstanceEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<PlayerNpcFavorEntity>().Property(e => e.FavorPoints).HasDefaultValue(0);
        modelBuilder.Entity<PlayerNpcFavorEntity>().Property(e => e.FavorLevel).HasDefaultValue(0);
        modelBuilder.Entity<DialogueDefinitionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<DialogueDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<DialogueDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<DialogueDefinitionEntity>().Property(e => e.Type).HasDefaultValue(DialogueType.Gossip);
        modelBuilder.Entity<DialogueDefinitionEntity>().Property(e => e.LanguageCode).HasDefaultValue("'en'");
        modelBuilder.Entity<DialogueLinkEntity>().Property(e => e.TargetType).HasConversion<string>();
        modelBuilder.Entity<DialogueLinkEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<DialogueLinkEntity>().Property(e => e.DialogueOrder).HasDefaultValue(0);
        modelBuilder.Entity<QuestDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<QuestDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<QuestDefinitionEntity>().Property(e => e.RequiredLevel).HasDefaultValue(1);
        modelBuilder.Entity<QuestDefinitionEntity>().Property(e => e.Repeatable).HasDefaultValue(false);
        modelBuilder.Entity<QuestDefinitionEntity>().Property(e => e.IsStoryQuest).HasDefaultValue(false);
        modelBuilder.Entity<QuestDefinitionEntity>().Property(e => e.IsDaily).HasDefaultValue(false);
        modelBuilder.Entity<QuestDefinitionEntity>().Property(e => e.IsWeekly).HasDefaultValue(false);
        modelBuilder.Entity<QuestDefinitionEntity>().Property(e => e.AutoAccept).HasDefaultValue(false);
        modelBuilder.Entity<QuestDefinitionEntity>().Property(e => e.AutoComplete).HasDefaultValue(false);
        modelBuilder.Entity<QuestConditionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<QuestConditionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<QuestConditionEntity>().Property(e => e.Sequence).HasDefaultValue(0);
        modelBuilder.Entity<QuestRewardEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<QuestRewardEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<QuestStepEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<QuestGroupEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<QuestGroupEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<QuestLogEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<QuestLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<QuestConditionProgressEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<QuestConditionProgressEntity>().Property(e => e.CurrentValue).HasDefaultValue(0);
        modelBuilder.Entity<QuestConditionProgressEntity>().Property(e => e.IsCompleted).HasDefaultValue(false);
        modelBuilder.Entity<QuestConditionProgressEntity>().Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<QuestRewardLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<QuestRewardLogEntity>().Property(e => e.GrantedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<AchievementDefinitionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<AchievementDefinitionEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<AchievementDefinitionEntity>().Property(e => e.RequiredLevel).HasDefaultValue(1);
        modelBuilder.Entity<AchievementDefinitionEntity>().Property(e => e.Points).HasDefaultValue(10);
        modelBuilder.Entity<AchievementDefinitionEntity>().Property(e => e.IsHidden).HasDefaultValue(false);
        modelBuilder.Entity<AchievementCategoryEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<AchievementCategoryEntity>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<AchievementCategoryEntity>().Property(e => e.DisplayOrder).HasDefaultValue(0);
        modelBuilder.Entity<AchievementConditionEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<AchievementConditionEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<AchievementConditionEntity>().Property(e => e.Sequence).HasDefaultValue(0);
        modelBuilder.Entity<AchievementRewardEntity>().Property(e => e.Type).HasConversion<string>();
        modelBuilder.Entity<AchievementRewardEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<AchievementLogEntity>().Property(e => e.Status).HasConversion<string>();
        modelBuilder.Entity<AchievementLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<AchievementConditionProgressEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<AchievementConditionProgressEntity>().Property(e => e.CurrentValue).HasDefaultValue(0);
        modelBuilder.Entity<AchievementConditionProgressEntity>().Property(e => e.IsCompleted).HasDefaultValue(false);
        modelBuilder.Entity<AchievementConditionProgressEntity>().Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        modelBuilder.Entity<AchievementRewardLogEntity>().Property(e => e.Id).HasDefaultValueSql("uuid_generate_v7()");
        modelBuilder.Entity<AchievementRewardLogEntity>().Property(e => e.GrantedAt).HasDefaultValueSql("now()");
        // 1:N Relationship: Account -> Auth
        modelBuilder.Entity<AuthEntity>()
            .HasOne(e => e.Account)
            .WithMany(e => e.Auths)
            .HasForeignKey(e => e.AccountId);
        // 1:N Relationship: Account -> Player
        modelBuilder.Entity<PlayerEntity>()
            .HasOne(e => e.Account)
            .WithMany(e => e.Players)
            .HasForeignKey(e => e.AccountId);
        // 1:N Relationship: Account -> GmAccount
        modelBuilder.Entity<GmAccountEntity>()
            .HasOne(e => e.Account)
            .WithMany(e => e.GmAccounts)
            .HasForeignKey(e => e.AccountId);
        // 1:N Relationship: MapDefinition -> MapEnvironment
        modelBuilder.Entity<MapEnvironmentEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.MapEnvironments)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> MapLayer
        modelBuilder.Entity<MapLayerEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.MapLayers)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> MapObjectInstance
        modelBuilder.Entity<MapObjectInstanceEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.MapObjectInstances)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> MapSpawnPoint
        modelBuilder.Entity<MapSpawnPointEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.MapSpawnPoints)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> MapZone
        modelBuilder.Entity<MapZoneEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.MapZones)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> EditableArea
        modelBuilder.Entity<EditableAreaEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.EditableAreas)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> MapDynamicObjectState
        modelBuilder.Entity<MapDynamicObjectStateEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.MapDynamicObjectStates)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> Portal
        modelBuilder.Entity<PortalEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.Portals)
            .HasForeignKey(e => e.FromMapId);
        // 1:N Relationship: MapDefinition -> Portal
        modelBuilder.Entity<PortalEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.Portals)
            .HasForeignKey(e => e.ToMapId);
        // 1:N Relationship: MapDefinition -> LandPlot
        modelBuilder.Entity<LandPlotEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.LandPlots)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> PlayerMonsterKillLog
        modelBuilder.Entity<PlayerMonsterKillLogEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.PlayerMonsterKillLogs)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> NpcInstance
        modelBuilder.Entity<NpcInstanceEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.NpcInstances)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> FishingSpotDefinition
        modelBuilder.Entity<FishingSpotDefinitionEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.FishingSpotDefinitions)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapDefinition -> FarmPlotDefinition
        modelBuilder.Entity<FarmPlotDefinitionEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.FarmPlotDefinitions)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: MapLayer -> MapTile
        modelBuilder.Entity<MapTileEntity>()
            .HasOne(e => e.MapLayer)
            .WithMany(e => e.MapTiles)
            .HasForeignKey(e => e.LayerId);
        // 1:N Relationship: MapZone -> ZoneEventTrigger
        modelBuilder.Entity<ZoneEventTriggerEntity>()
            .HasOne(e => e.MapZone)
            .WithMany(e => e.ZoneEventTriggers)
            .HasForeignKey(e => e.ZoneId);
        // 1:N Relationship: MapZone -> NpcInstance
        modelBuilder.Entity<NpcInstanceEntity>()
            .HasOne(e => e.MapZone)
            .WithMany(e => e.NpcInstances)
            .HasForeignKey(e => e.ZoneId);
        // 1:N Relationship: MapZone -> FishingSpotDefinition
        modelBuilder.Entity<FishingSpotDefinitionEntity>()
            .HasOne(e => e.MapZone)
            .WithMany(e => e.FishingSpotDefinitions)
            .HasForeignKey(e => e.ZoneId);
        // 1:N Relationship: MapDefinition -> MapEventTrigger
        modelBuilder.Entity<MapEventTriggerEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.MapEventTriggers)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: Player -> MapPlayerState
        modelBuilder.Entity<MapPlayerStateEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.MapPlayerStates)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: MapDefinition -> MapPlayerState
        modelBuilder.Entity<MapPlayerStateEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.MapPlayerStates)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: DropTableDefinition -> DropTableDetail
        modelBuilder.Entity<DropTableDetailEntity>()
            .HasOne(e => e.DropTableDefinition)
            .WithMany(e => e.DropTableDetails)
            .HasForeignKey(e => e.DropTableId);
        // 1:N Relationship: DropTableDefinition -> EntityDefinition
        modelBuilder.Entity<EntityDefinitionEntity>()
            .HasOne(e => e.DropTableDefinition)
            .WithMany(e => e.EntityDefinitions)
            .HasForeignKey(e => e.DropTableId);
        // 1:N Relationship: ItemDefinition -> DropTableDetail
        modelBuilder.Entity<DropTableDetailEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.DropTableDetails)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: EntityDefinition -> MapObjectInstance
        modelBuilder.Entity<MapObjectInstanceEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.MapObjectInstances)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> MapSpawnPoint
        modelBuilder.Entity<MapSpawnPointEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.MapSpawnPoints)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> CropDefinition
        modelBuilder.Entity<CropDefinitionEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.CropDefinitions)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> LivestockDefinition
        modelBuilder.Entity<LivestockDefinitionEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.LivestockDefinitions)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> MonsterDefinition
        modelBuilder.Entity<MonsterDefinitionEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.MonsterDefinitions)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> OreDefinition
        modelBuilder.Entity<OreDefinitionEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.OreDefinitions)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> LogDefinition
        modelBuilder.Entity<LogDefinitionEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.LogDefinitions)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> BuildingDefinition
        modelBuilder.Entity<BuildingDefinitionEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.BuildingDefinitions)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> NpcDefinition
        modelBuilder.Entity<NpcDefinitionEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.NpcDefinitions)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> PetDefinition
        modelBuilder.Entity<PetDefinitionEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.PetDefinitions)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: EntityDefinition -> FishDefinition
        modelBuilder.Entity<FishDefinitionEntity>()
            .HasOne(e => e.EntityDefinition)
            .WithMany(e => e.FishDefinitions)
            .HasForeignKey(e => e.EntityDefinitionId);
        // 1:N Relationship: StatDefinition -> StatValue
        modelBuilder.Entity<StatValueEntity>()
            .HasOne(e => e.StatDefinition)
            .WithMany(e => e.StatValues)
            .HasForeignKey(e => e.StatDefinitionId);
        // 1:N Relationship: ItemEffectDefinition -> ItemDefinition
        modelBuilder.Entity<ItemDefinitionEntity>()
            .HasOne(e => e.ItemEffectDefinition)
            .WithMany(e => e.ItemDefinitions)
            .HasForeignKey(e => e.UseEffectId);
        // 1:N Relationship: ItemDefinition -> ItemInstance
        modelBuilder.Entity<ItemInstanceEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.ItemInstances)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: Container -> ItemInstance
        modelBuilder.Entity<ItemInstanceEntity>()
            .HasOne(e => e.Container)
            .WithMany(e => e.ItemInstances)
            .HasForeignKey(e => e.ContainerId);
        // 1:N Relationship: ItemInstance -> PlayerInventoryItem
        modelBuilder.Entity<PlayerInventoryItemEntity>()
            .HasOne(e => e.ItemInstance)
            .WithMany(e => e.PlayerInventoryItems)
            .HasForeignKey(e => e.ItemInstanceId);
        // 1:N Relationship: Player -> ItemAcquisitionLog
        modelBuilder.Entity<ItemAcquisitionLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.ItemAcquisitionLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: ItemInstance -> ItemAcquisitionLog
        modelBuilder.Entity<ItemAcquisitionLogEntity>()
            .HasOne(e => e.ItemInstance)
            .WithMany(e => e.ItemAcquisitionLogs)
            .HasForeignKey(e => e.ItemInstanceId);
        // 1:N Relationship: ItemDefinition -> ItemAcquisitionLog
        modelBuilder.Entity<ItemAcquisitionLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.ItemAcquisitionLogs)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: Player -> ItemEnhancementLog
        modelBuilder.Entity<ItemEnhancementLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.ItemEnhancementLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: ItemInstance -> ItemEnhancementLog
        modelBuilder.Entity<ItemEnhancementLogEntity>()
            .HasOne(e => e.ItemInstance)
            .WithMany(e => e.ItemEnhancementLogs)
            .HasForeignKey(e => e.ItemInstanceId);
        // 1:N Relationship: ItemDefinition -> ItemEnhancementLog
        modelBuilder.Entity<ItemEnhancementLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.ItemEnhancementLogs)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: Player -> PlayerInventoryTabLimit
        modelBuilder.Entity<PlayerInventoryTabLimitEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerInventoryTabLimits)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> PlayerInventoryItem
        modelBuilder.Entity<PlayerInventoryItemEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerInventoryItems)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: RecipeDefinition -> RecipeIngredient
        modelBuilder.Entity<RecipeIngredientEntity>()
            .HasOne(e => e.RecipeDefinition)
            .WithMany(e => e.RecipeIngredients)
            .HasForeignKey(e => e.RecipeId);
        // 1:N Relationship: ItemDefinition -> RecipeIngredient
        modelBuilder.Entity<RecipeIngredientEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.RecipeIngredients)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: RecipeDefinition -> RecipeResult
        modelBuilder.Entity<RecipeResultEntity>()
            .HasOne(e => e.RecipeDefinition)
            .WithMany(e => e.RecipeResults)
            .HasForeignKey(e => e.RecipeId);
        // 1:N Relationship: ItemDefinition -> RecipeResult
        modelBuilder.Entity<RecipeResultEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.RecipeResults)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: Player -> PlayerCraftingLog
        modelBuilder.Entity<PlayerCraftingLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerCraftingLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: RecipeDefinition -> PlayerCraftingLog
        modelBuilder.Entity<PlayerCraftingLogEntity>()
            .HasOne(e => e.RecipeDefinition)
            .WithMany(e => e.PlayerCraftingLogs)
            .HasForeignKey(e => e.RecipeId);
        // 1:N Relationship: ItemInstance -> PlayerCraftingLog
        modelBuilder.Entity<PlayerCraftingLogEntity>()
            .HasOne(e => e.ItemInstance)
            .WithMany(e => e.PlayerCraftingLogs)
            .HasForeignKey(e => e.ResultItemInstanceId);
        // 1:N Relationship: ItemDefinition -> PlayerCraftingLog
        modelBuilder.Entity<PlayerCraftingLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.PlayerCraftingLogs)
            .HasForeignKey(e => e.ResultItemDefinitionId);
        // 1:N Relationship: Player -> PlayerRecipe
        modelBuilder.Entity<PlayerRecipeEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerRecipes)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: RecipeDefinition -> PlayerRecipe
        modelBuilder.Entity<PlayerRecipeEntity>()
            .HasOne(e => e.RecipeDefinition)
            .WithMany(e => e.PlayerRecipes)
            .HasForeignKey(e => e.RecipeDefinitionId);
        // 1:N Relationship: ItemDefinition -> RecipeDefinition
        modelBuilder.Entity<RecipeDefinitionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.RecipeDefinitions)
            .HasForeignKey(e => e.FailRewardItemId);
        // 1:N Relationship: RecipeDefinition -> CookingRecipeDefinition
        modelBuilder.Entity<CookingRecipeDefinitionEntity>()
            .HasOne(e => e.RecipeDefinition)
            .WithMany(e => e.CookingRecipeDefinitions)
            .HasForeignKey(e => e.RecipeDefinitionId);
        // 1:N Relationship: CookingRecipeDefinition -> CookingResultDefinition
        modelBuilder.Entity<CookingResultDefinitionEntity>()
            .HasOne(e => e.CookingRecipeDefinition)
            .WithMany(e => e.CookingResultDefinitions)
            .HasForeignKey(e => e.RecipeId);
        // 1:N Relationship: ItemDefinition -> CookingResultDefinition
        modelBuilder.Entity<CookingResultDefinitionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.CookingResultDefinitions)
            .HasForeignKey(e => e.ResultItemId);
        // 1:N Relationship: CookingRecipeDefinition -> CookingGradeRewardDefinition
        modelBuilder.Entity<CookingGradeRewardDefinitionEntity>()
            .HasOne(e => e.CookingRecipeDefinition)
            .WithMany(e => e.CookingGradeRewardDefinitions)
            .HasForeignKey(e => e.RecipeId);
        // 1:N Relationship: ItemDefinition -> CookingGradeRewardDefinition
        modelBuilder.Entity<CookingGradeRewardDefinitionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.CookingGradeRewardDefinitions)
            .HasForeignKey(e => e.RewardItemId);
        // 1:N Relationship: CookingRecipeDefinition -> CookingStepDefinition
        modelBuilder.Entity<CookingStepDefinitionEntity>()
            .HasOne(e => e.CookingRecipeDefinition)
            .WithMany(e => e.CookingStepDefinitions)
            .HasForeignKey(e => e.RecipeId);
        // 1:N Relationship: Player -> PlayerCookingLog
        modelBuilder.Entity<PlayerCookingLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerCookingLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: CookingRecipeDefinition -> PlayerCookingLog
        modelBuilder.Entity<PlayerCookingLogEntity>()
            .HasOne(e => e.CookingRecipeDefinition)
            .WithMany(e => e.PlayerCookingLogs)
            .HasForeignKey(e => e.CookingRecipeId);
        // 1:N Relationship: Player -> PortalUseLog
        modelBuilder.Entity<PortalUseLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PortalUseLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Portal -> PortalUseLog
        modelBuilder.Entity<PortalUseLogEntity>()
            .HasOne(e => e.Portal)
            .WithMany(e => e.PortalUseLogs)
            .HasForeignKey(e => e.PortalId);
        // 1:N Relationship: ItemDefinition -> SeedGradeToCropGradeMap
        modelBuilder.Entity<SeedGradeToCropGradeMapEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.SeedGradeToCropGradeMaps)
            .HasForeignKey(e => e.SeedItemId);
        // 1:N Relationship: ItemDefinition -> SeedGradeToCropGradeMap
        modelBuilder.Entity<SeedGradeToCropGradeMapEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.SeedGradeToCropGradeMaps)
            .HasForeignKey(e => e.FertilizerItemId);
        // 1:N Relationship: CropDefinition -> SeedGradeToCropGradeMap
        modelBuilder.Entity<SeedGradeToCropGradeMapEntity>()
            .HasOne(e => e.CropDefinition)
            .WithMany(e => e.SeedGradeToCropGradeMaps)
            .HasForeignKey(e => e.CropDefinitionId);
        // 1:N Relationship: CropDefinition -> CropProduct
        modelBuilder.Entity<CropProductEntity>()
            .HasOne(e => e.CropDefinition)
            .WithMany(e => e.CropProducts)
            .HasForeignKey(e => e.CropDefinitionId);
        // 1:N Relationship: ItemDefinition -> CropProduct
        modelBuilder.Entity<CropProductEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.CropProducts)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: Player -> PlayerCropInstance
        modelBuilder.Entity<PlayerCropInstanceEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerCropInstances)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: CropDefinition -> PlayerCropInstance
        modelBuilder.Entity<PlayerCropInstanceEntity>()
            .HasOne(e => e.CropDefinition)
            .WithMany(e => e.PlayerCropInstances)
            .HasForeignKey(e => e.CropDefinitionId);
        // 1:N Relationship: PlayerCropInstance -> PlayerCropHarvestLog
        modelBuilder.Entity<PlayerCropHarvestLogEntity>()
            .HasOne(e => e.PlayerCropInstance)
            .WithMany(e => e.PlayerCropHarvestLogs)
            .HasForeignKey(e => e.PlayerCropInstanceId);
        // 1:N Relationship: Player -> PlayerCropHarvestLog
        modelBuilder.Entity<PlayerCropHarvestLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerCropHarvestLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: CropDefinition -> PlayerCropHarvestLog
        modelBuilder.Entity<PlayerCropHarvestLogEntity>()
            .HasOne(e => e.CropDefinition)
            .WithMany(e => e.PlayerCropHarvestLogs)
            .HasForeignKey(e => e.CropDefinitionId);
        // 1:N Relationship: BuildingDefinition -> FarmPlotDefinition
        modelBuilder.Entity<FarmPlotDefinitionEntity>()
            .HasOne(e => e.BuildingDefinition)
            .WithMany(e => e.FarmPlotDefinitions)
            .HasForeignKey(e => e.BuildingDefinitionId);
        // 1:N Relationship: FishDefinition -> FishHabitat
        modelBuilder.Entity<FishHabitatEntity>()
            .HasOne(e => e.FishDefinition)
            .WithMany(e => e.FishHabitats)
            .HasForeignKey(e => e.FishDefinitionId);
        // 1:N Relationship: FishingSpotDefinition -> FishHabitat
        modelBuilder.Entity<FishHabitatEntity>()
            .HasOne(e => e.FishingSpotDefinition)
            .WithMany(e => e.FishHabitats)
            .HasForeignKey(e => e.FishingSpotId);
        // 1:N Relationship: ItemDefinition -> FishHabitat
        modelBuilder.Entity<FishHabitatEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.FishHabitats)
            .HasForeignKey(e => e.RequiredBaitItemId);
        // 1:N Relationship: Player -> PlayerFishingLog
        modelBuilder.Entity<PlayerFishingLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerFishingLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: FishingSpotDefinition -> PlayerFishingLog
        modelBuilder.Entity<PlayerFishingLogEntity>()
            .HasOne(e => e.FishingSpotDefinition)
            .WithMany(e => e.PlayerFishingLogs)
            .HasForeignKey(e => e.FishingSpotId);
        // 1:N Relationship: FishDefinition -> PlayerFishingLog
        modelBuilder.Entity<PlayerFishingLogEntity>()
            .HasOne(e => e.FishDefinition)
            .WithMany(e => e.PlayerFishingLogs)
            .HasForeignKey(e => e.FishDefinitionId);
        // 1:N Relationship: ItemDefinition -> PlayerFishingLog
        modelBuilder.Entity<PlayerFishingLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.PlayerFishingLogs)
            .HasForeignKey(e => e.BaitItemId);
        // 1:N Relationship: ItemDefinition -> PlayerFishingLog
        modelBuilder.Entity<PlayerFishingLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.PlayerFishingLogs)
            .HasForeignKey(e => e.RodItemId);
        // 1:N Relationship: ItemDefinition -> FishDefinition
        modelBuilder.Entity<FishDefinitionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.FishDefinitions)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: ItemDefinition -> LivestockDefinition
        modelBuilder.Entity<LivestockDefinitionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.LivestockDefinitions)
            .HasForeignKey(e => e.ProductItemId);
        // 1:N Relationship: Player -> PlayerLivestockInstance
        modelBuilder.Entity<PlayerLivestockInstanceEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerLivestockInstances)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: LivestockDefinition -> PlayerLivestockInstance
        modelBuilder.Entity<PlayerLivestockInstanceEntity>()
            .HasOne(e => e.LivestockDefinition)
            .WithMany(e => e.PlayerLivestockInstances)
            .HasForeignKey(e => e.LivestockDefinitionId);
        // 1:N Relationship: PlayerLivestockInstance -> PlayerLivestockProductLog
        modelBuilder.Entity<PlayerLivestockProductLogEntity>()
            .HasOne(e => e.PlayerLivestockInstance)
            .WithMany(e => e.PlayerLivestockProductLogs)
            .HasForeignKey(e => e.PlayerLivestockInstanceId);
        // 1:N Relationship: Player -> PlayerLivestockProductLog
        modelBuilder.Entity<PlayerLivestockProductLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerLivestockProductLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: ItemDefinition -> PlayerLivestockProductLog
        modelBuilder.Entity<PlayerLivestockProductLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.PlayerLivestockProductLogs)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: PlayerLivestockInstance -> PlayerLivestockFeedLog
        modelBuilder.Entity<PlayerLivestockFeedLogEntity>()
            .HasOne(e => e.PlayerLivestockInstance)
            .WithMany(e => e.PlayerLivestockFeedLogs)
            .HasForeignKey(e => e.PlayerLivestockInstanceId);
        // 1:N Relationship: Player -> PlayerLivestockFeedLog
        modelBuilder.Entity<PlayerLivestockFeedLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerLivestockFeedLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: ItemDefinition -> PlayerLivestockFeedLog
        modelBuilder.Entity<PlayerLivestockFeedLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.PlayerLivestockFeedLogs)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: PlayerLivestockInstance -> PlayerLivestockDiseaseLog
        modelBuilder.Entity<PlayerLivestockDiseaseLogEntity>()
            .HasOne(e => e.PlayerLivestockInstance)
            .WithMany(e => e.PlayerLivestockDiseaseLogs)
            .HasForeignKey(e => e.PlayerLivestockInstanceId);
        // 1:N Relationship: PlayerLivestockInstance -> PlayerLivestockGradeLog
        modelBuilder.Entity<PlayerLivestockGradeLogEntity>()
            .HasOne(e => e.PlayerLivestockInstance)
            .WithMany(e => e.PlayerLivestockGradeLogs)
            .HasForeignKey(e => e.PlayerLivestockInstanceId);
        // 1:N Relationship: PlayerLivestockInstance -> PlayerLivestockGrowthLog
        modelBuilder.Entity<PlayerLivestockGrowthLogEntity>()
            .HasOne(e => e.PlayerLivestockInstance)
            .WithMany(e => e.PlayerLivestockGrowthLogs)
            .HasForeignKey(e => e.PlayerLivestockInstanceId);
        // 1:N Relationship: MonsterAiFsmDefinition -> MonsterDefinition
        modelBuilder.Entity<MonsterDefinitionEntity>()
            .HasOne(e => e.MonsterAiFsmDefinition)
            .WithMany(e => e.MonsterDefinitions)
            .HasForeignKey(e => e.FsmId);
        // 1:N Relationship: MonsterAiBtDefinition -> MonsterDefinition
        modelBuilder.Entity<MonsterDefinitionEntity>()
            .HasOne(e => e.MonsterAiBtDefinition)
            .WithMany(e => e.MonsterDefinitions)
            .HasForeignKey(e => e.BtId);
        // 1:N Relationship: Player -> PlayerMonsterKillLog
        modelBuilder.Entity<PlayerMonsterKillLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerMonsterKillLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: MonsterDefinition -> PlayerMonsterKillLog
        modelBuilder.Entity<PlayerMonsterKillLogEntity>()
            .HasOne(e => e.MonsterDefinition)
            .WithMany(e => e.PlayerMonsterKillLogs)
            .HasForeignKey(e => e.MonsterDefinitionId);
        // 1:N Relationship: Player -> PlayerMiningLog
        modelBuilder.Entity<PlayerMiningLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerMiningLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: OreDefinition -> PlayerMiningLog
        modelBuilder.Entity<PlayerMiningLogEntity>()
            .HasOne(e => e.OreDefinition)
            .WithMany(e => e.PlayerMiningLogs)
            .HasForeignKey(e => e.OreDefinitionId);
        // 1:N Relationship: ItemDefinition -> PlayerMiningLog
        modelBuilder.Entity<PlayerMiningLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.PlayerMiningLogs)
            .HasForeignKey(e => e.ToolItemId);
        // 1:N Relationship: Player -> PlayerLoggingLog
        modelBuilder.Entity<PlayerLoggingLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerLoggingLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: LogDefinition -> PlayerLoggingLog
        modelBuilder.Entity<PlayerLoggingLogEntity>()
            .HasOne(e => e.LogDefinition)
            .WithMany(e => e.PlayerLoggingLogs)
            .HasForeignKey(e => e.LogDefinitionId);
        // 1:N Relationship: ItemDefinition -> PlayerLoggingLog
        modelBuilder.Entity<PlayerLoggingLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.PlayerLoggingLogs)
            .HasForeignKey(e => e.ToolItemId);
        // 1:N Relationship: Player -> PlayerLevelLog
        modelBuilder.Entity<PlayerLevelLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerLevelLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: LevelDefinition -> PlayerLevelLog
        modelBuilder.Entity<PlayerLevelLogEntity>()
            .HasOne(e => e.LevelDefinition)
            .WithMany(e => e.PlayerLevelLogs)
            .HasForeignKey(e => e.LevelCode);
        // 1:N Relationship: Player -> PlayerStatLog
        modelBuilder.Entity<PlayerStatLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerStatLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: StatDefinition -> PlayerStatLog
        modelBuilder.Entity<PlayerStatLogEntity>()
            .HasOne(e => e.StatDefinition)
            .WithMany(e => e.PlayerStatLogs)
            .HasForeignKey(e => e.StatDefinitionId);
        // 1:N Relationship: MiniGameDefinition -> MiniGameRewardDefinition
        modelBuilder.Entity<MiniGameRewardDefinitionEntity>()
            .HasOne(e => e.MiniGameDefinition)
            .WithMany(e => e.MiniGameRewardDefinitions)
            .HasForeignKey(e => e.MinigameId);
        // 1:N Relationship: ItemDefinition -> MiniGameRewardDefinition
        modelBuilder.Entity<MiniGameRewardDefinitionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.MiniGameRewardDefinitions)
            .HasForeignKey(e => e.RewardItemId);
        // 1:N Relationship: Player -> PlayerMiniGameResult
        modelBuilder.Entity<PlayerMiniGameResultEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerMiniGameResults)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: MiniGameDefinition -> PlayerMiniGameResult
        modelBuilder.Entity<PlayerMiniGameResultEntity>()
            .HasOne(e => e.MiniGameDefinition)
            .WithMany(e => e.PlayerMiniGameResults)
            .HasForeignKey(e => e.MinigameId);
        // 1:N Relationship: ItemDefinition -> PlayerMiniGameResult
        modelBuilder.Entity<PlayerMiniGameResultEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.PlayerMiniGameResults)
            .HasForeignKey(e => e.RewardItemId);
        // 1:N Relationship: JobDefinition -> JobSkillDefinition
        modelBuilder.Entity<JobSkillDefinitionEntity>()
            .HasOne(e => e.JobDefinition)
            .WithMany(e => e.JobSkillDefinitions)
            .HasForeignKey(e => e.JobId);
        // 1:N Relationship: Player -> PlayerJobSkill
        modelBuilder.Entity<PlayerJobSkillEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerJobSkills)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: JobSkillDefinition -> PlayerJobSkill
        modelBuilder.Entity<PlayerJobSkillEntity>()
            .HasOne(e => e.JobSkillDefinition)
            .WithMany(e => e.PlayerJobSkills)
            .HasForeignKey(e => e.SkillId);
        // 1:N Relationship: JobSkillDefinition -> JobSkillLevelDefinition
        modelBuilder.Entity<JobSkillLevelDefinitionEntity>()
            .HasOne(e => e.JobSkillDefinition)
            .WithMany(e => e.JobSkillLevelDefinitions)
            .HasForeignKey(e => e.SkillId);
        // 1:N Relationship: ItemDefinition -> JobSkillLevelDefinition
        modelBuilder.Entity<JobSkillLevelDefinitionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.JobSkillLevelDefinitions)
            .HasForeignKey(e => e.RewardItemId);
        // 1:N Relationship: Player -> PlayerJobSkillPoint
        modelBuilder.Entity<PlayerJobSkillPointEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerJobSkillPoints)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: JobDefinition -> PlayerJobSkillPoint
        modelBuilder.Entity<PlayerJobSkillPointEntity>()
            .HasOne(e => e.JobDefinition)
            .WithMany(e => e.PlayerJobSkillPoints)
            .HasForeignKey(e => e.JobId);
        // 1:N Relationship: JobDefinition -> JobLevelDefinition
        modelBuilder.Entity<JobLevelDefinitionEntity>()
            .HasOne(e => e.JobDefinition)
            .WithMany(e => e.JobLevelDefinitions)
            .HasForeignKey(e => e.JobId);
        // 1:N Relationship: ItemDefinition -> JobLevelDefinition
        modelBuilder.Entity<JobLevelDefinitionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.JobLevelDefinitions)
            .HasForeignKey(e => e.RewardItemId);
        // 1:N Relationship: JobDefinition -> JobSkillTree
        modelBuilder.Entity<JobSkillTreeEntity>()
            .HasOne(e => e.JobDefinition)
            .WithMany(e => e.JobSkillTrees)
            .HasForeignKey(e => e.JobId);
        // 1:N Relationship: JobSkillDefinition -> JobSkillTree
        modelBuilder.Entity<JobSkillTreeEntity>()
            .HasOne(e => e.JobSkillDefinition)
            .WithMany(e => e.JobSkillTrees)
            .HasForeignKey(e => e.ParentSkillId);
        // 1:N Relationship: JobSkillDefinition -> JobSkillTree
        modelBuilder.Entity<JobSkillTreeEntity>()
            .HasOne(e => e.JobSkillDefinition)
            .WithMany(e => e.JobSkillTrees)
            .HasForeignKey(e => e.ChildSkillId);
        // 1:N Relationship: JobDefinition -> JobTree
        modelBuilder.Entity<JobTreeEntity>()
            .HasOne(e => e.JobDefinition)
            .WithMany(e => e.JobTrees)
            .HasForeignKey(e => e.ParentJobId);
        // 1:N Relationship: JobDefinition -> JobTree
        modelBuilder.Entity<JobTreeEntity>()
            .HasOne(e => e.JobDefinition)
            .WithMany(e => e.JobTrees)
            .HasForeignKey(e => e.ChildJobId);
        // 1:N Relationship: SkillDefinition -> SkillEffectDefinition
        modelBuilder.Entity<SkillEffectDefinitionEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.SkillEffectDefinitions)
            .HasForeignKey(e => e.SkillDefinitionId);
        // 1:N Relationship: SkillDefinition -> SkillTree
        modelBuilder.Entity<SkillTreeEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.SkillTrees)
            .HasForeignKey(e => e.ParentSkillId);
        // 1:N Relationship: SkillDefinition -> SkillTree
        modelBuilder.Entity<SkillTreeEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.SkillTrees)
            .HasForeignKey(e => e.ChildSkillId);
        // 1:N Relationship: Player -> PlayerSkill
        modelBuilder.Entity<PlayerSkillEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerSkills)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: SkillDefinition -> PlayerSkill
        modelBuilder.Entity<PlayerSkillEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.PlayerSkills)
            .HasForeignKey(e => e.SkillDefinitionId);
        // 1:N Relationship: Player -> PlayerSkillLog
        modelBuilder.Entity<PlayerSkillLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerSkillLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: SkillDefinition -> PlayerSkillLog
        modelBuilder.Entity<PlayerSkillLogEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.PlayerSkillLogs)
            .HasForeignKey(e => e.SkillDefinitionId);
        // 1:N Relationship: Player -> PlayerQuickSlotPreset
        modelBuilder.Entity<PlayerQuickSlotPresetEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerQuickSlotPresets)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: PlayerQuickSlotPreset -> PlayerQuickSlot
        modelBuilder.Entity<PlayerQuickSlotEntity>()
            .HasOne(e => e.PlayerQuickSlotPreset)
            .WithMany(e => e.PlayerQuickSlots)
            .HasForeignKey(e => e.PresetId);
        // 1:N Relationship: SkillDefinition -> PlayerQuickSlot
        modelBuilder.Entity<PlayerQuickSlotEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.PlayerQuickSlots)
            .HasForeignKey(e => e.SkillDefinitionId);
        // 1:N Relationship: ItemDefinition -> PlayerQuickSlot
        modelBuilder.Entity<PlayerQuickSlotEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.PlayerQuickSlots)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: ClassDefinition -> ClassTree
        modelBuilder.Entity<ClassTreeEntity>()
            .HasOne(e => e.ClassDefinition)
            .WithMany(e => e.ClassTrees)
            .HasForeignKey(e => e.ParentClassId);
        // 1:N Relationship: ClassDefinition -> ClassTree
        modelBuilder.Entity<ClassTreeEntity>()
            .HasOne(e => e.ClassDefinition)
            .WithMany(e => e.ClassTrees)
            .HasForeignKey(e => e.ChildClassId);
        // 1:N Relationship: Player -> PlayerClass
        modelBuilder.Entity<PlayerClassEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerClasses)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: ClassDefinition -> PlayerClass
        modelBuilder.Entity<PlayerClassEntity>()
            .HasOne(e => e.ClassDefinition)
            .WithMany(e => e.PlayerClasses)
            .HasForeignKey(e => e.ClassId);
        // 1:N Relationship: ClassDefinition -> ClassEquipmentRestriction
        modelBuilder.Entity<ClassEquipmentRestrictionEntity>()
            .HasOne(e => e.ClassDefinition)
            .WithMany(e => e.ClassEquipmentRestrictions)
            .HasForeignKey(e => e.ClassId);
        // 1:N Relationship: Tag -> ClassEquipmentRestriction
        modelBuilder.Entity<ClassEquipmentRestrictionEntity>()
            .HasOne(e => e.Tag)
            .WithMany(e => e.ClassEquipmentRestrictions)
            .HasForeignKey(e => e.ItemTagId);
        // 1:N Relationship: ClassDefinition -> ClassTraitDefinition
        modelBuilder.Entity<ClassTraitDefinitionEntity>()
            .HasOne(e => e.ClassDefinition)
            .WithMany(e => e.ClassTraitDefinitions)
            .HasForeignKey(e => e.ClassId);
        // 1:N Relationship: ClassDefinition -> ClassSkillDefinition
        modelBuilder.Entity<ClassSkillDefinitionEntity>()
            .HasOne(e => e.ClassDefinition)
            .WithMany(e => e.ClassSkillDefinitions)
            .HasForeignKey(e => e.ClassId);
        // 1:N Relationship: SkillDefinition -> ClassSkillDefinition
        modelBuilder.Entity<ClassSkillDefinitionEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.ClassSkillDefinitions)
            .HasForeignKey(e => e.SkillDefinitionId);
        // 1:N Relationship: ClassDefinition -> ClassSkillTree
        modelBuilder.Entity<ClassSkillTreeEntity>()
            .HasOne(e => e.ClassDefinition)
            .WithMany(e => e.ClassSkillTrees)
            .HasForeignKey(e => e.ClassId);
        // 1:N Relationship: SkillDefinition -> ClassSkillTree
        modelBuilder.Entity<ClassSkillTreeEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.ClassSkillTrees)
            .HasForeignKey(e => e.ParentSkillId);
        // 1:N Relationship: SkillDefinition -> ClassSkillTree
        modelBuilder.Entity<ClassSkillTreeEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.ClassSkillTrees)
            .HasForeignKey(e => e.ChildSkillId);
        // 1:N Relationship: Player -> PlayerClassChangeLog
        modelBuilder.Entity<PlayerClassChangeLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerClassChangeLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: ClassDefinition -> PlayerClassChangeLog
        modelBuilder.Entity<PlayerClassChangeLogEntity>()
            .HasOne(e => e.ClassDefinition)
            .WithMany(e => e.PlayerClassChangeLogs)
            .HasForeignKey(e => e.OldClassId);
        // 1:N Relationship: ClassDefinition -> PlayerClassChangeLog
        modelBuilder.Entity<PlayerClassChangeLogEntity>()
            .HasOne(e => e.ClassDefinition)
            .WithMany(e => e.PlayerClassChangeLogs)
            .HasForeignKey(e => e.NewClassId);
        // 1:N Relationship: Player -> PlayerSkillChangeLog
        modelBuilder.Entity<PlayerSkillChangeLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerSkillChangeLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: SkillDefinition -> PlayerSkillChangeLog
        modelBuilder.Entity<PlayerSkillChangeLogEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.PlayerSkillChangeLogs)
            .HasForeignKey(e => e.SkillDefinitionId);
        // 1:N Relationship: Player -> ChatLog
        modelBuilder.Entity<ChatLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.ChatLogs)
            .HasForeignKey(e => e.SenderId);
        // 1:N Relationship: Player -> ChatLog
        modelBuilder.Entity<ChatLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.ChatLogs)
            .HasForeignKey(e => e.ReceiverId);
        // 1:N Relationship: ChannelDefinition -> ChatLog
        modelBuilder.Entity<ChatLogEntity>()
            .HasOne(e => e.ChannelDefinition)
            .WithMany(e => e.ChatLogs)
            .HasForeignKey(e => e.ChannelCode);
        // 1:N Relationship: PartyDefinition -> ChatLog
        modelBuilder.Entity<ChatLogEntity>()
            .HasOne(e => e.PartyDefinition)
            .WithMany(e => e.ChatLogs)
            .HasForeignKey(e => e.PartyId);
        // 1:N Relationship: GuildDefinition -> ChatLog
        modelBuilder.Entity<ChatLogEntity>()
            .HasOne(e => e.GuildDefinition)
            .WithMany(e => e.ChatLogs)
            .HasForeignKey(e => e.GuildId);
        // 1:N Relationship: Player -> CommunityReportLog
        modelBuilder.Entity<CommunityReportLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.CommunityReportLogs)
            .HasForeignKey(e => e.ReporterId);
        // 1:N Relationship: Player -> CommunityReportLog
        modelBuilder.Entity<CommunityReportLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.CommunityReportLogs)
            .HasForeignKey(e => e.ReportedId);
        // 1:N Relationship: Player -> CommunityReportLog
        modelBuilder.Entity<CommunityReportLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.CommunityReportLogs)
            .HasForeignKey(e => e.ResolverId);
        // 1:N Relationship: ChatLog -> CommunityReportLog
        modelBuilder.Entity<CommunityReportLogEntity>()
            .HasOne(e => e.ChatLog)
            .WithMany(e => e.CommunityReportLogs)
            .HasForeignKey(e => e.ChatLogId);
        // 1:N Relationship: Player -> ModerationActionLog
        modelBuilder.Entity<ModerationActionLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.ModerationActionLogs)
            .HasForeignKey(e => e.TargetPlayerId);
        // 1:N Relationship: Player -> ModerationActionLog
        modelBuilder.Entity<ModerationActionLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.ModerationActionLogs)
            .HasForeignKey(e => e.ModeratorId);
        // 1:N Relationship: CommunityReportLog -> ModerationActionLog
        modelBuilder.Entity<ModerationActionLogEntity>()
            .HasOne(e => e.CommunityReportLog)
            .WithMany(e => e.ModerationActionLogs)
            .HasForeignKey(e => e.RelatedReportId);
        // 1:N Relationship: ChatLog -> ModerationActionLog
        modelBuilder.Entity<ModerationActionLogEntity>()
            .HasOne(e => e.ChatLog)
            .WithMany(e => e.ModerationActionLogs)
            .HasForeignKey(e => e.RelatedChatLogId);
        // 1:N Relationship: Player -> FriendRelation
        modelBuilder.Entity<FriendRelationEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.FriendRelations)
            .HasForeignKey(e => e.PlayerId1);
        // 1:N Relationship: Player -> FriendRelation
        modelBuilder.Entity<FriendRelationEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.FriendRelations)
            .HasForeignKey(e => e.PlayerId2);
        // 1:N Relationship: Player -> PartyDefinition
        modelBuilder.Entity<PartyDefinitionEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PartyDefinitions)
            .HasForeignKey(e => e.LeaderId);
        // 1:N Relationship: PartyDefinition -> PartyMember
        modelBuilder.Entity<PartyMemberEntity>()
            .HasOne(e => e.PartyDefinition)
            .WithMany(e => e.PartyMembers)
            .HasForeignKey(e => e.PartyId);
        // 1:N Relationship: Player -> PartyMember
        modelBuilder.Entity<PartyMemberEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PartyMembers)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> CommunityNotice
        modelBuilder.Entity<CommunityNoticeEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.CommunityNotices)
            .HasForeignKey(e => e.AuthorId);
        // 1:N Relationship: Player -> GuildDefinition
        modelBuilder.Entity<GuildDefinitionEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.GuildDefinitions)
            .HasForeignKey(e => e.LeaderId);
        // 1:N Relationship: GuildDefinition -> GuildRole
        modelBuilder.Entity<GuildRoleEntity>()
            .HasOne(e => e.GuildDefinition)
            .WithMany(e => e.GuildRoles)
            .HasForeignKey(e => e.GuildId);
        // 1:N Relationship: GuildDefinition -> GuildMember
        modelBuilder.Entity<GuildMemberEntity>()
            .HasOne(e => e.GuildDefinition)
            .WithMany(e => e.GuildMembers)
            .HasForeignKey(e => e.GuildId);
        // 1:N Relationship: Player -> GuildMember
        modelBuilder.Entity<GuildMemberEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.GuildMembers)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: GuildRole -> GuildMember
        modelBuilder.Entity<GuildMemberEntity>()
            .HasOne(e => e.GuildRole)
            .WithMany(e => e.GuildMembers)
            .HasForeignKey(e => e.RoleId);
        // 1:N Relationship: GuildDefinition -> GuildNotice
        modelBuilder.Entity<GuildNoticeEntity>()
            .HasOne(e => e.GuildDefinition)
            .WithMany(e => e.GuildNotices)
            .HasForeignKey(e => e.GuildId);
        // 1:N Relationship: Player -> GuildNotice
        modelBuilder.Entity<GuildNoticeEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.GuildNotices)
            .HasForeignKey(e => e.AuthorId);
        // 1:N Relationship: GuildDefinition -> GuildJoinRequest
        modelBuilder.Entity<GuildJoinRequestEntity>()
            .HasOne(e => e.GuildDefinition)
            .WithMany(e => e.GuildJoinRequests)
            .HasForeignKey(e => e.GuildId);
        // 1:N Relationship: Player -> GuildJoinRequest
        modelBuilder.Entity<GuildJoinRequestEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.GuildJoinRequests)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> GuildJoinRequest
        modelBuilder.Entity<GuildJoinRequestEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.GuildJoinRequests)
            .HasForeignKey(e => e.ProcessedBy);
        // 1:N Relationship: GuildDefinition -> GuildContributionLog
        modelBuilder.Entity<GuildContributionLogEntity>()
            .HasOne(e => e.GuildDefinition)
            .WithMany(e => e.GuildContributionLogs)
            .HasForeignKey(e => e.GuildId);
        // 1:N Relationship: Player -> GuildContributionLog
        modelBuilder.Entity<GuildContributionLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.GuildContributionLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> Recommendation
        modelBuilder.Entity<RecommendationEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.Recommendations)
            .HasForeignKey(e => e.FromPlayerId);
        // 1:N Relationship: Recommendation -> RecommendationLog
        modelBuilder.Entity<RecommendationLogEntity>()
            .HasOne(e => e.Recommendation)
            .WithMany(e => e.RecommendationLogs)
            .HasForeignKey(e => e.RecommendationId);
        // 1:N Relationship: Player -> RecommendationLog
        modelBuilder.Entity<RecommendationLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.RecommendationLogs)
            .HasForeignKey(e => e.ActorPlayerId);
        // 1:N Relationship: TitleCategory -> TitleDefinition
        modelBuilder.Entity<TitleDefinitionEntity>()
            .HasOne(e => e.TitleCategory)
            .WithMany(e => e.TitleDefinitions)
            .HasForeignKey(e => e.CategoryId);
        // 1:N Relationship: TitleDefinition -> TitleEffect
        modelBuilder.Entity<TitleEffectEntity>()
            .HasOne(e => e.TitleDefinition)
            .WithMany(e => e.TitleEffects)
            .HasForeignKey(e => e.TitleId);
        // 1:N Relationship: TitleDefinition -> TitleUnlockCondition
        modelBuilder.Entity<TitleUnlockConditionEntity>()
            .HasOne(e => e.TitleDefinition)
            .WithMany(e => e.TitleUnlockConditions)
            .HasForeignKey(e => e.TitleId);
        // 1:N Relationship: Player -> PlayerTitle
        modelBuilder.Entity<PlayerTitleEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerTitles)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: TitleDefinition -> PlayerTitle
        modelBuilder.Entity<PlayerTitleEntity>()
            .HasOne(e => e.TitleDefinition)
            .WithMany(e => e.PlayerTitles)
            .HasForeignKey(e => e.TitleId);
        // 1:N Relationship: Player -> PlayerTitleSlot
        modelBuilder.Entity<PlayerTitleSlotEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerTitleSlots)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: PlayerTitle -> PlayerTitleSlot
        modelBuilder.Entity<PlayerTitleSlotEntity>()
            .HasOne(e => e.PlayerTitle)
            .WithMany(e => e.PlayerTitleSlots)
            .HasForeignKey(e => e.PlayerTitleId);
        // 1:N Relationship: Player -> TitleUnlockHistory
        modelBuilder.Entity<TitleUnlockHistoryEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.TitleUnlockHistories)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: TitleDefinition -> TitleUnlockHistory
        modelBuilder.Entity<TitleUnlockHistoryEntity>()
            .HasOne(e => e.TitleDefinition)
            .WithMany(e => e.TitleUnlockHistories)
            .HasForeignKey(e => e.TitleId);
        // 1:N Relationship: LandPlot -> PlayerLand
        modelBuilder.Entity<PlayerLandEntity>()
            .HasOne(e => e.LandPlot)
            .WithMany(e => e.PlayerLands)
            .HasForeignKey(e => e.LandPlotId);
        // 1:N Relationship: Player -> Notification
        modelBuilder.Entity<NotificationEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.Notifications)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: GuildDefinition -> GuildTitle
        modelBuilder.Entity<GuildTitleEntity>()
            .HasOne(e => e.GuildDefinition)
            .WithMany(e => e.GuildTitles)
            .HasForeignKey(e => e.GuildId);
        // 1:N Relationship: TitleDefinition -> GuildTitle
        modelBuilder.Entity<GuildTitleEntity>()
            .HasOne(e => e.TitleDefinition)
            .WithMany(e => e.GuildTitles)
            .HasForeignKey(e => e.TitleId);
        // 1:N Relationship: Player -> PlayerBuff
        modelBuilder.Entity<PlayerBuffEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerBuffs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: BuffDefinition -> PlayerBuff
        modelBuilder.Entity<PlayerBuffEntity>()
            .HasOne(e => e.BuffDefinition)
            .WithMany(e => e.PlayerBuffs)
            .HasForeignKey(e => e.BuffDefinitionId);
        // 1:N Relationship: Player -> PlayerBuffLog
        modelBuilder.Entity<PlayerBuffLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerBuffLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: BuffDefinition -> PlayerBuffLog
        modelBuilder.Entity<PlayerBuffLogEntity>()
            .HasOne(e => e.BuffDefinition)
            .WithMany(e => e.PlayerBuffLogs)
            .HasForeignKey(e => e.BuffDefinitionId);
        // 1:N Relationship: SkillDefinition -> SkillGrantedBuff
        modelBuilder.Entity<SkillGrantedBuffEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.SkillGrantedBuffs)
            .HasForeignKey(e => e.SkillDefinitionId);
        // 1:N Relationship: BuffDefinition -> SkillGrantedBuff
        modelBuilder.Entity<SkillGrantedBuffEntity>()
            .HasOne(e => e.BuffDefinition)
            .WithMany(e => e.SkillGrantedBuffs)
            .HasForeignKey(e => e.BuffDefinitionId);
        // 1:N Relationship: Player -> PlayerPet
        modelBuilder.Entity<PlayerPetEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerPets)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: PetDefinition -> PlayerPet
        modelBuilder.Entity<PlayerPetEntity>()
            .HasOne(e => e.PetDefinition)
            .WithMany(e => e.PlayerPets)
            .HasForeignKey(e => e.PetDefinitionId);
        // 1:N Relationship: PetDefinition -> PetSkill
        modelBuilder.Entity<PetSkillEntity>()
            .HasOne(e => e.PetDefinition)
            .WithMany(e => e.PetSkills)
            .HasForeignKey(e => e.PetDefinitionId);
        // 1:N Relationship: SkillDefinition -> PetSkill
        modelBuilder.Entity<PetSkillEntity>()
            .HasOne(e => e.SkillDefinition)
            .WithMany(e => e.PetSkills)
            .HasForeignKey(e => e.SkillDefinitionId);
        // 1:N Relationship: PlayerPet -> PlayerPetSkill
        modelBuilder.Entity<PlayerPetSkillEntity>()
            .HasOne(e => e.PlayerPet)
            .WithMany(e => e.PlayerPetSkills)
            .HasForeignKey(e => e.PlayerPetId);
        // 1:N Relationship: PetSkill -> PlayerPetSkill
        modelBuilder.Entity<PlayerPetSkillEntity>()
            .HasOne(e => e.PetSkill)
            .WithMany(e => e.PlayerPetSkills)
            .HasForeignKey(e => e.PetSkillId);
        // 1:N Relationship: Mail -> MailAttachment
        modelBuilder.Entity<MailAttachmentEntity>()
            .HasOne(e => e.Mail)
            .WithMany(e => e.MailAttachments)
            .HasForeignKey(e => e.MailId);
        // 1:N Relationship: ItemDefinition -> MailAttachment
        modelBuilder.Entity<MailAttachmentEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.MailAttachments)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: ItemInstance -> MailAttachment
        modelBuilder.Entity<MailAttachmentEntity>()
            .HasOne(e => e.ItemInstance)
            .WithMany(e => e.MailAttachments)
            .HasForeignKey(e => e.ItemInstanceId);
        // 1:N Relationship: CurrencyDefinition -> MailAttachment
        modelBuilder.Entity<MailAttachmentEntity>()
            .HasOne(e => e.CurrencyDefinition)
            .WithMany(e => e.MailAttachments)
            .HasForeignKey(e => e.CurrencyCode);
        // 1:N Relationship: Mail -> MailReadLog
        modelBuilder.Entity<MailReadLogEntity>()
            .HasOne(e => e.Mail)
            .WithMany(e => e.MailReadLogs)
            .HasForeignKey(e => e.MailId);
        // 1:N Relationship: Mail -> MailDeleteLog
        modelBuilder.Entity<MailDeleteLogEntity>()
            .HasOne(e => e.Mail)
            .WithMany(e => e.MailDeleteLogs)
            .HasForeignKey(e => e.MailId);
        // 1:N Relationship: EventDefinition -> PlayerEventProgress
        modelBuilder.Entity<PlayerEventProgressEntity>()
            .HasOne(e => e.EventDefinition)
            .WithMany(e => e.PlayerEventProgresses)
            .HasForeignKey(e => e.EventId);
        // 1:N Relationship: Player -> PlayerEventProgress
        modelBuilder.Entity<PlayerEventProgressEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerEventProgresses)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: EventDefinition -> EventReward
        modelBuilder.Entity<EventRewardEntity>()
            .HasOne(e => e.EventDefinition)
            .WithMany(e => e.EventRewards)
            .HasForeignKey(e => e.EventId);
        // 1:N Relationship: EventReward -> PlayerEventRewardLog
        modelBuilder.Entity<PlayerEventRewardLogEntity>()
            .HasOne(e => e.EventReward)
            .WithMany(e => e.PlayerEventRewardLogs)
            .HasForeignKey(e => e.EventRewardId);
        // 1:N Relationship: Player -> PlayerEventRewardLog
        modelBuilder.Entity<PlayerEventRewardLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerEventRewardLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: SeasonPassDefinition -> SeasonPassMission
        modelBuilder.Entity<SeasonPassMissionEntity>()
            .HasOne(e => e.SeasonPassDefinition)
            .WithMany(e => e.SeasonPassMissions)
            .HasForeignKey(e => e.SeasonPassId);
        // 1:N Relationship: SeasonPassDefinition -> PlayerSeasonPass
        modelBuilder.Entity<PlayerSeasonPassEntity>()
            .HasOne(e => e.SeasonPassDefinition)
            .WithMany(e => e.PlayerSeasonPasses)
            .HasForeignKey(e => e.SeasonPassId);
        // 1:N Relationship: Player -> PlayerSeasonPass
        modelBuilder.Entity<PlayerSeasonPassEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerSeasonPasses)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: PlayerSeasonPass -> PlayerSeasonPassMission
        modelBuilder.Entity<PlayerSeasonPassMissionEntity>()
            .HasOne(e => e.PlayerSeasonPass)
            .WithMany(e => e.PlayerSeasonPassMissions)
            .HasForeignKey(e => e.PlayerSeasonPassId);
        // 1:N Relationship: SeasonPassMission -> PlayerSeasonPassMission
        modelBuilder.Entity<PlayerSeasonPassMissionEntity>()
            .HasOne(e => e.SeasonPassMission)
            .WithMany(e => e.PlayerSeasonPassMissions)
            .HasForeignKey(e => e.SeasonPassMissionId);
        // 1:N Relationship: SeasonPassDefinition -> SeasonPassReward
        modelBuilder.Entity<SeasonPassRewardEntity>()
            .HasOne(e => e.SeasonPassDefinition)
            .WithMany(e => e.SeasonPassRewards)
            .HasForeignKey(e => e.SeasonPassId);
        // 1:N Relationship: MailAttachment -> SeasonPassReward
        modelBuilder.Entity<SeasonPassRewardEntity>()
            .HasOne(e => e.MailAttachment)
            .WithMany(e => e.SeasonPassRewards)
            .HasForeignKey(e => e.MailAttachmentId);
        // 1:N Relationship: SeasonPassReward -> PlayerSeasonPassRewardLog
        modelBuilder.Entity<PlayerSeasonPassRewardLogEntity>()
            .HasOne(e => e.SeasonPassReward)
            .WithMany(e => e.PlayerSeasonPassRewardLogs)
            .HasForeignKey(e => e.SeasonPassRewardId);
        // 1:N Relationship: Player -> PlayerSeasonPassRewardLog
        modelBuilder.Entity<PlayerSeasonPassRewardLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerSeasonPassRewardLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: SeasonPassDefinition -> SeasonPassPurchaseLog
        modelBuilder.Entity<SeasonPassPurchaseLogEntity>()
            .HasOne(e => e.SeasonPassDefinition)
            .WithMany(e => e.SeasonPassPurchaseLogs)
            .HasForeignKey(e => e.SeasonPassId);
        // 1:N Relationship: Player -> SeasonPassPurchaseLog
        modelBuilder.Entity<SeasonPassPurchaseLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.SeasonPassPurchaseLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> SeasonPassMissionLog
        modelBuilder.Entity<SeasonPassMissionLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.SeasonPassMissionLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: SeasonPassDefinition -> SeasonPassMissionLog
        modelBuilder.Entity<SeasonPassMissionLogEntity>()
            .HasOne(e => e.SeasonPassDefinition)
            .WithMany(e => e.SeasonPassMissionLogs)
            .HasForeignKey(e => e.SeasonPassId);
        // 1:N Relationship: SeasonPassMission -> SeasonPassMissionLog
        modelBuilder.Entity<SeasonPassMissionLogEntity>()
            .HasOne(e => e.SeasonPassMission)
            .WithMany(e => e.SeasonPassMissionLogs)
            .HasForeignKey(e => e.MissionId);
        // 1:N Relationship: NotificationSchedule -> NotificationLog
        modelBuilder.Entity<NotificationLogEntity>()
            .HasOne(e => e.NotificationSchedule)
            .WithMany(e => e.NotificationLogs)
            .HasForeignKey(e => e.NotificationScheduleId);
        // 1:N Relationship: Player -> PlayerNotificationLog
        modelBuilder.Entity<PlayerNotificationLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerNotificationLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> PlayerBuilding
        modelBuilder.Entity<PlayerBuildingEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerBuildings)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: PlayerLand -> PlayerBuilding
        modelBuilder.Entity<PlayerBuildingEntity>()
            .HasOne(e => e.PlayerLand)
            .WithMany(e => e.PlayerBuildings)
            .HasForeignKey(e => e.LandPlotId);
        // 1:N Relationship: BuildingDefinition -> PlayerBuilding
        modelBuilder.Entity<PlayerBuildingEntity>()
            .HasOne(e => e.BuildingDefinition)
            .WithMany(e => e.PlayerBuildings)
            .HasForeignKey(e => e.BuildingDefinitionId);
        // 1:N Relationship: BuildingDefinition -> BuildingUpgradeDefinition
        modelBuilder.Entity<BuildingUpgradeDefinitionEntity>()
            .HasOne(e => e.BuildingDefinition)
            .WithMany(e => e.BuildingUpgradeDefinitions)
            .HasForeignKey(e => e.BuildingDefinitionId);
        // 1:N Relationship: BuildingUpgradeDefinition -> BuildingUpgradeEffect
        modelBuilder.Entity<BuildingUpgradeEffectEntity>()
            .HasOne(e => e.BuildingUpgradeDefinition)
            .WithMany(e => e.BuildingUpgradeEffects)
            .HasForeignKey(e => e.UpgradeDefinitionId);
        // 1:N Relationship: Player -> BuildingActionLog
        modelBuilder.Entity<BuildingActionLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.BuildingActionLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: PlayerBuilding -> BuildingActionLog
        modelBuilder.Entity<BuildingActionLogEntity>()
            .HasOne(e => e.PlayerBuilding)
            .WithMany(e => e.BuildingActionLogs)
            .HasForeignKey(e => e.PlayerBuildingId);
        // 1:N Relationship: PlayerBuilding -> PlayerBuildingSlot
        modelBuilder.Entity<PlayerBuildingSlotEntity>()
            .HasOne(e => e.PlayerBuilding)
            .WithMany(e => e.PlayerBuildingSlots)
            .HasForeignKey(e => e.PlayerBuildingId);
        // 1:N Relationship: ItemInstance -> PlayerBuildingSlot
        modelBuilder.Entity<PlayerBuildingSlotEntity>()
            .HasOne(e => e.ItemInstance)
            .WithMany(e => e.PlayerBuildingSlots)
            .HasForeignKey(e => e.ItemInstanceId);
        // 1:N Relationship: PlayerLivestockInstance -> PlayerBuildingSlot
        modelBuilder.Entity<PlayerBuildingSlotEntity>()
            .HasOne(e => e.PlayerLivestockInstance)
            .WithMany(e => e.PlayerBuildingSlots)
            .HasForeignKey(e => e.LivestockInstanceId);
        // 1:N Relationship: PlayerCropInstance -> PlayerBuildingSlot
        modelBuilder.Entity<PlayerBuildingSlotEntity>()
            .HasOne(e => e.PlayerCropInstance)
            .WithMany(e => e.PlayerBuildingSlots)
            .HasForeignKey(e => e.CropInstanceId);
        // 1:N Relationship: Player -> MarketListing
        modelBuilder.Entity<MarketListingEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.MarketListings)
            .HasForeignKey(e => e.SellerId);
        // 1:N Relationship: Player -> MarketListing
        modelBuilder.Entity<MarketListingEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.MarketListings)
            .HasForeignKey(e => e.BuyerId);
        // 1:N Relationship: ItemDefinition -> MarketListing
        modelBuilder.Entity<MarketListingEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.MarketListings)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: ItemInstance -> MarketListing
        modelBuilder.Entity<MarketListingEntity>()
            .HasOne(e => e.ItemInstance)
            .WithMany(e => e.MarketListings)
            .HasForeignKey(e => e.ItemInstanceId);
        // 1:N Relationship: CurrencyDefinition -> MarketListing
        modelBuilder.Entity<MarketListingEntity>()
            .HasOne(e => e.CurrencyDefinition)
            .WithMany(e => e.MarketListings)
            .HasForeignKey(e => e.CurrencyCode);
        // 1:N Relationship: MarketListing -> MarketTransaction
        modelBuilder.Entity<MarketTransactionEntity>()
            .HasOne(e => e.MarketListing)
            .WithMany(e => e.MarketTransactions)
            .HasForeignKey(e => e.MarketListingId);
        // 1:N Relationship: Player -> MarketTransaction
        modelBuilder.Entity<MarketTransactionEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.MarketTransactions)
            .HasForeignKey(e => e.BuyerId);
        // 1:N Relationship: Player -> MarketTransaction
        modelBuilder.Entity<MarketTransactionEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.MarketTransactions)
            .HasForeignKey(e => e.SellerId);
        // 1:N Relationship: ItemDefinition -> MarketTransaction
        modelBuilder.Entity<MarketTransactionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.MarketTransactions)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: ItemInstance -> MarketTransaction
        modelBuilder.Entity<MarketTransactionEntity>()
            .HasOne(e => e.ItemInstance)
            .WithMany(e => e.MarketTransactions)
            .HasForeignKey(e => e.ItemInstanceId);
        // 1:N Relationship: CurrencyDefinition -> MarketTransaction
        modelBuilder.Entity<MarketTransactionEntity>()
            .HasOne(e => e.CurrencyDefinition)
            .WithMany(e => e.MarketTransactions)
            .HasForeignKey(e => e.CurrencyCode);
        // 1:N Relationship: Player -> Trade
        modelBuilder.Entity<TradeEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.Trades)
            .HasForeignKey(e => e.Player1Id);
        // 1:N Relationship: Player -> Trade
        modelBuilder.Entity<TradeEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.Trades)
            .HasForeignKey(e => e.Player2Id);
        // 1:N Relationship: CurrencyDefinition -> Trade
        modelBuilder.Entity<TradeEntity>()
            .HasOne(e => e.CurrencyDefinition)
            .WithMany(e => e.Trades)
            .HasForeignKey(e => e.CurrencyCode);
        // 1:N Relationship: Trade -> TradeItem
        modelBuilder.Entity<TradeItemEntity>()
            .HasOne(e => e.Trade)
            .WithMany(e => e.TradeItems)
            .HasForeignKey(e => e.TradeId);
        // 1:N Relationship: Player -> TradeItem
        modelBuilder.Entity<TradeItemEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.TradeItems)
            .HasForeignKey(e => e.OfferingPlayerId);
        // 1:N Relationship: ItemInstance -> TradeItem
        modelBuilder.Entity<TradeItemEntity>()
            .HasOne(e => e.ItemInstance)
            .WithMany(e => e.TradeItems)
            .HasForeignKey(e => e.ItemInstanceId);
        // 1:N Relationship: Player -> CurrencyTransactionLog
        modelBuilder.Entity<CurrencyTransactionLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.CurrencyTransactionLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: CurrencyDefinition -> CurrencyTransactionLog
        modelBuilder.Entity<CurrencyTransactionLogEntity>()
            .HasOne(e => e.CurrencyDefinition)
            .WithMany(e => e.CurrencyTransactionLogs)
            .HasForeignKey(e => e.CurrencyCode);
        // 1:N Relationship: BoardGameDefinition -> BoardGameRoom
        modelBuilder.Entity<BoardGameRoomEntity>()
            .HasOne(e => e.BoardGameDefinition)
            .WithMany(e => e.BoardGameRooms)
            .HasForeignKey(e => e.BoardGameId);
        // 1:N Relationship: Player -> BoardGameRoom
        modelBuilder.Entity<BoardGameRoomEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.BoardGameRooms)
            .HasForeignKey(e => e.HostPlayerId);
        // 1:N Relationship: Player -> BoardGameRoom
        modelBuilder.Entity<BoardGameRoomEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.BoardGameRooms)
            .HasForeignKey(e => e.WinnerPlayerId);
        // 1:N Relationship: BoardGameRoom -> BoardGameParticipant
        modelBuilder.Entity<BoardGameParticipantEntity>()
            .HasOne(e => e.BoardGameRoom)
            .WithMany(e => e.BoardGameParticipants)
            .HasForeignKey(e => e.RoomId);
        // 1:N Relationship: Player -> BoardGameParticipant
        modelBuilder.Entity<BoardGameParticipantEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.BoardGameParticipants)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: BoardGameRoom -> BoardGameTurnLog
        modelBuilder.Entity<BoardGameTurnLogEntity>()
            .HasOne(e => e.BoardGameRoom)
            .WithMany(e => e.BoardGameTurnLogs)
            .HasForeignKey(e => e.RoomId);
        // 1:N Relationship: Player -> BoardGameTurnLog
        modelBuilder.Entity<BoardGameTurnLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.BoardGameTurnLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> UserPattern
        modelBuilder.Entity<UserPatternEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserPatterns)
            .HasForeignKey(e => e.CreatorId);
        // 1:N Relationship: UserPattern -> UserPatternLike
        modelBuilder.Entity<UserPatternLikeEntity>()
            .HasOne(e => e.UserPattern)
            .WithMany(e => e.UserPatternLikes)
            .HasForeignKey(e => e.PatternId);
        // 1:N Relationship: Player -> UserPatternLike
        modelBuilder.Entity<UserPatternLikeEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserPatternLikes)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: UserPattern -> UserPatternComment
        modelBuilder.Entity<UserPatternCommentEntity>()
            .HasOne(e => e.UserPattern)
            .WithMany(e => e.UserPatternComments)
            .HasForeignKey(e => e.PatternId);
        // 1:N Relationship: Player -> UserPatternComment
        modelBuilder.Entity<UserPatternCommentEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserPatternComments)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> UserRoom
        modelBuilder.Entity<UserRoomEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserRooms)
            .HasForeignKey(e => e.OwnerId);
        // 1:N Relationship: UserRoom -> UserRoomParticipant
        modelBuilder.Entity<UserRoomParticipantEntity>()
            .HasOne(e => e.UserRoom)
            .WithMany(e => e.UserRoomParticipants)
            .HasForeignKey(e => e.RoomId);
        // 1:N Relationship: Player -> UserRoomParticipant
        modelBuilder.Entity<UserRoomParticipantEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserRoomParticipants)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: UserRoom -> UserRoomChat
        modelBuilder.Entity<UserRoomChatEntity>()
            .HasOne(e => e.UserRoom)
            .WithMany(e => e.UserRoomChats)
            .HasForeignKey(e => e.RoomId);
        // 1:N Relationship: Player -> UserRoomChat
        modelBuilder.Entity<UserRoomChatEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserRoomChats)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> UserContentReport
        modelBuilder.Entity<UserContentReportEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserContentReports)
            .HasForeignKey(e => e.ReporterId);
        // 1:N Relationship: Player -> UserRoomFavorite
        modelBuilder.Entity<UserRoomFavoriteEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserRoomFavorites)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: UserRoom -> UserRoomFavorite
        modelBuilder.Entity<UserRoomFavoriteEntity>()
            .HasOne(e => e.UserRoom)
            .WithMany(e => e.UserRoomFavorites)
            .HasForeignKey(e => e.RoomId);
        // 1:N Relationship: Player -> UserPatternFavorite
        modelBuilder.Entity<UserPatternFavoriteEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserPatternFavorites)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: UserPattern -> UserPatternFavorite
        modelBuilder.Entity<UserPatternFavoriteEntity>()
            .HasOne(e => e.UserPattern)
            .WithMany(e => e.UserPatternFavorites)
            .HasForeignKey(e => e.PatternId);
        // 1:N Relationship: UserPatternBoard -> UserPatternBoardPost
        modelBuilder.Entity<UserPatternBoardPostEntity>()
            .HasOne(e => e.UserPatternBoard)
            .WithMany(e => e.UserPatternBoardPosts)
            .HasForeignKey(e => e.BoardId);
        // 1:N Relationship: UserPattern -> UserPatternBoardPost
        modelBuilder.Entity<UserPatternBoardPostEntity>()
            .HasOne(e => e.UserPattern)
            .WithMany(e => e.UserPatternBoardPosts)
            .HasForeignKey(e => e.PatternId);
        // 1:N Relationship: Player -> UserPatternBoardPost
        modelBuilder.Entity<UserPatternBoardPostEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserPatternBoardPosts)
            .HasForeignKey(e => e.CreatorId);
        // 1:N Relationship: UserPatternBoardPost -> UserPatternBoardPostComment
        modelBuilder.Entity<UserPatternBoardPostCommentEntity>()
            .HasOne(e => e.UserPatternBoardPost)
            .WithMany(e => e.UserPatternBoardPostComments)
            .HasForeignKey(e => e.PostId);
        // 1:N Relationship: Player -> UserPatternBoardPostComment
        modelBuilder.Entity<UserPatternBoardPostCommentEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.UserPatternBoardPostComments)
            .HasForeignKey(e => e.CommenterId);
        // 1:N Relationship: Portal -> DungeonDefinition
        modelBuilder.Entity<DungeonDefinitionEntity>()
            .HasOne(e => e.Portal)
            .WithMany(e => e.DungeonDefinitions)
            .HasForeignKey(e => e.EntryPortalId);
        // 1:N Relationship: DungeonDefinition -> DungeonZoneLink
        modelBuilder.Entity<DungeonZoneLinkEntity>()
            .HasOne(e => e.DungeonDefinition)
            .WithMany(e => e.DungeonZoneLinks)
            .HasForeignKey(e => e.DungeonId);
        // 1:N Relationship: MapZone -> DungeonZoneLink
        modelBuilder.Entity<DungeonZoneLinkEntity>()
            .HasOne(e => e.MapZone)
            .WithMany(e => e.DungeonZoneLinks)
            .HasForeignKey(e => e.MapZoneId);
        // 1:N Relationship: DungeonDefinition -> DungeonRun
        modelBuilder.Entity<DungeonRunEntity>()
            .HasOne(e => e.DungeonDefinition)
            .WithMany(e => e.DungeonRuns)
            .HasForeignKey(e => e.DungeonId);
        // 1:N Relationship: PartyDefinition -> DungeonRun
        modelBuilder.Entity<DungeonRunEntity>()
            .HasOne(e => e.PartyDefinition)
            .WithMany(e => e.DungeonRuns)
            .HasForeignKey(e => e.PartyId);
        // 1:N Relationship: Player -> DungeonRun
        modelBuilder.Entity<DungeonRunEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.DungeonRuns)
            .HasForeignKey(e => e.LeaderId);
        // 1:N Relationship: MapZone -> DungeonRun
        modelBuilder.Entity<DungeonRunEntity>()
            .HasOne(e => e.MapZone)
            .WithMany(e => e.DungeonRuns)
            .HasForeignKey(e => e.CurrentZoneId);
        // 1:N Relationship: DungeonRun -> DungeonRunParticipant
        modelBuilder.Entity<DungeonRunParticipantEntity>()
            .HasOne(e => e.DungeonRun)
            .WithMany(e => e.DungeonRunParticipants)
            .HasForeignKey(e => e.DungeonRunId);
        // 1:N Relationship: Player -> DungeonRunParticipant
        modelBuilder.Entity<DungeonRunParticipantEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.DungeonRunParticipants)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Player -> SecurityIncidentLog
        modelBuilder.Entity<SecurityIncidentLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.SecurityIncidentLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Account -> SecurityIncidentLog
        modelBuilder.Entity<SecurityIncidentLogEntity>()
            .HasOne(e => e.Account)
            .WithMany(e => e.SecurityIncidentLogs)
            .HasForeignKey(e => e.AccountId);
        // 1:N Relationship: Player -> PlayerBan
        modelBuilder.Entity<PlayerBanEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerBans)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: Account -> PlayerBan
        modelBuilder.Entity<PlayerBanEntity>()
            .HasOne(e => e.Account)
            .WithMany(e => e.PlayerBans)
            .HasForeignKey(e => e.AccountId);
        // 1:N Relationship: SecurityIncidentLog -> PlayerBan
        modelBuilder.Entity<PlayerBanEntity>()
            .HasOne(e => e.SecurityIncidentLog)
            .WithMany(e => e.PlayerBans)
            .HasForeignKey(e => e.RelatedIncidentId);
        // 1:N Relationship: GmRole -> GmAccount
        modelBuilder.Entity<GmAccountEntity>()
            .HasOne(e => e.GmRole)
            .WithMany(e => e.GmAccounts)
            .HasForeignKey(e => e.GmRoleId);
        // 1:N Relationship: GmAccount -> GmActionLog
        modelBuilder.Entity<GmActionLogEntity>()
            .HasOne(e => e.GmAccount)
            .WithMany(e => e.GmActionLogs)
            .HasForeignKey(e => e.GmAccountId);
        // 1:N Relationship: GmAccount -> GmNotice
        modelBuilder.Entity<GmNoticeEntity>()
            .HasOne(e => e.GmAccount)
            .WithMany(e => e.GmNotices)
            .HasForeignKey(e => e.GmAccountId);
        // 1:N Relationship: Player -> PlayerWorldEventParticipation
        modelBuilder.Entity<PlayerWorldEventParticipationEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerWorldEventParticipations)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: WorldEvent -> PlayerWorldEventParticipation
        modelBuilder.Entity<PlayerWorldEventParticipationEntity>()
            .HasOne(e => e.WorldEvent)
            .WithMany(e => e.PlayerWorldEventParticipations)
            .HasForeignKey(e => e.EventId);
        // 1:N Relationship: MapDefinition -> WorldEvent
        modelBuilder.Entity<WorldEventEntity>()
            .HasOne(e => e.MapDefinition)
            .WithMany(e => e.WorldEvents)
            .HasForeignKey(e => e.MapId);
        // 1:N Relationship: ShopDefinition -> ShopItemDefinition
        modelBuilder.Entity<ShopItemDefinitionEntity>()
            .HasOne(e => e.ShopDefinition)
            .WithMany(e => e.ShopItemDefinitions)
            .HasForeignKey(e => e.ShopId);
        // 1:N Relationship: ItemDefinition -> ShopItemDefinition
        modelBuilder.Entity<ShopItemDefinitionEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.ShopItemDefinitions)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: Player -> PlayerShopPurchaseLimit
        modelBuilder.Entity<PlayerShopPurchaseLimitEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerShopPurchaseLimits)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: ShopItemDefinition -> PlayerShopPurchaseLimit
        modelBuilder.Entity<PlayerShopPurchaseLimitEntity>()
            .HasOne(e => e.ShopItemDefinition)
            .WithMany(e => e.PlayerShopPurchaseLimits)
            .HasForeignKey(e => e.ShopItemId);
        // 1:N Relationship: Player -> ShopTransactionLog
        modelBuilder.Entity<ShopTransactionLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.ShopTransactionLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: ShopDefinition -> ShopTransactionLog
        modelBuilder.Entity<ShopTransactionLogEntity>()
            .HasOne(e => e.ShopDefinition)
            .WithMany(e => e.ShopTransactionLogs)
            .HasForeignKey(e => e.ShopId);
        // 1:N Relationship: ShopItemDefinition -> ShopTransactionLog
        modelBuilder.Entity<ShopTransactionLogEntity>()
            .HasOne(e => e.ShopItemDefinition)
            .WithMany(e => e.ShopTransactionLogs)
            .HasForeignKey(e => e.ShopItemId);
        // 1:N Relationship: ItemDefinition -> ShopTransactionLog
        modelBuilder.Entity<ShopTransactionLogEntity>()
            .HasOne(e => e.ItemDefinition)
            .WithMany(e => e.ShopTransactionLogs)
            .HasForeignKey(e => e.ItemDefinitionId);
        // 1:N Relationship: ShopDefinition -> NpcDefinition
        modelBuilder.Entity<NpcDefinitionEntity>()
            .HasOne(e => e.ShopDefinition)
            .WithMany(e => e.NpcDefinitions)
            .HasForeignKey(e => e.ShopId);
        // 1:N Relationship: Player -> PlayerNpcFavor
        modelBuilder.Entity<PlayerNpcFavorEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.PlayerNpcFavors)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: NpcDefinition -> PlayerNpcFavor
        modelBuilder.Entity<PlayerNpcFavorEntity>()
            .HasOne(e => e.NpcDefinition)
            .WithMany(e => e.PlayerNpcFavors)
            .HasForeignKey(e => e.NpcDefinitionId);
        // 1:N Relationship: DialogueDefinition -> DialogueLink
        modelBuilder.Entity<DialogueLinkEntity>()
            .HasOne(e => e.DialogueDefinition)
            .WithMany(e => e.DialogueLinks)
            .HasForeignKey(e => e.SourceDialogueCode);
        // 1:N Relationship: DialogueDefinition -> DialogueLink
        modelBuilder.Entity<DialogueLinkEntity>()
            .HasOne(e => e.DialogueDefinition)
            .WithMany(e => e.DialogueLinks)
            .HasForeignKey(e => e.TargetDialogueCode);
        // 1:N Relationship: QuestGroup -> QuestDefinition
        modelBuilder.Entity<QuestDefinitionEntity>()
            .HasOne(e => e.QuestGroup)
            .WithMany(e => e.QuestDefinitions)
            .HasForeignKey(e => e.GroupId);
        // 1:N Relationship: NpcDefinition -> QuestDefinition
        modelBuilder.Entity<QuestDefinitionEntity>()
            .HasOne(e => e.NpcDefinition)
            .WithMany(e => e.QuestDefinitions)
            .HasForeignKey(e => e.StartNpcCode);
        // 1:N Relationship: NpcDefinition -> QuestDefinition
        modelBuilder.Entity<QuestDefinitionEntity>()
            .HasOne(e => e.NpcDefinition)
            .WithMany(e => e.QuestDefinitions)
            .HasForeignKey(e => e.EndNpcCode);
        // 1:N Relationship: QuestDefinition -> QuestCondition
        modelBuilder.Entity<QuestConditionEntity>()
            .HasOne(e => e.QuestDefinition)
            .WithMany(e => e.QuestConditions)
            .HasForeignKey(e => e.QuestId);
        // 1:N Relationship: QuestDefinition -> QuestReward
        modelBuilder.Entity<QuestRewardEntity>()
            .HasOne(e => e.QuestDefinition)
            .WithMany(e => e.QuestRewards)
            .HasForeignKey(e => e.QuestId);
        // 1:N Relationship: QuestDefinition -> QuestStep
        modelBuilder.Entity<QuestStepEntity>()
            .HasOne(e => e.QuestDefinition)
            .WithMany(e => e.QuestSteps)
            .HasForeignKey(e => e.QuestId);
        // 1:N Relationship: Player -> QuestLog
        modelBuilder.Entity<QuestLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.QuestLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: QuestDefinition -> QuestLog
        modelBuilder.Entity<QuestLogEntity>()
            .HasOne(e => e.QuestDefinition)
            .WithMany(e => e.QuestLogs)
            .HasForeignKey(e => e.QuestId);
        // 1:N Relationship: QuestLog -> QuestConditionProgress
        modelBuilder.Entity<QuestConditionProgressEntity>()
            .HasOne(e => e.QuestLog)
            .WithMany(e => e.QuestConditionProgresses)
            .HasForeignKey(e => e.QuestLogId);
        // 1:N Relationship: QuestCondition -> QuestConditionProgress
        modelBuilder.Entity<QuestConditionProgressEntity>()
            .HasOne(e => e.QuestCondition)
            .WithMany(e => e.QuestConditionProgresses)
            .HasForeignKey(e => e.QuestConditionId);
        // 1:N Relationship: QuestLog -> QuestRewardLog
        modelBuilder.Entity<QuestRewardLogEntity>()
            .HasOne(e => e.QuestLog)
            .WithMany(e => e.QuestRewardLogs)
            .HasForeignKey(e => e.QuestLogId);
        // 1:N Relationship: QuestReward -> QuestRewardLog
        modelBuilder.Entity<QuestRewardLogEntity>()
            .HasOne(e => e.QuestReward)
            .WithMany(e => e.QuestRewardLogs)
            .HasForeignKey(e => e.QuestRewardId);
        // 1:N Relationship: AchievementCategory -> AchievementDefinition
        modelBuilder.Entity<AchievementDefinitionEntity>()
            .HasOne(e => e.AchievementCategory)
            .WithMany(e => e.AchievementDefinitions)
            .HasForeignKey(e => e.CategoryId);
        // 1:N Relationship: AchievementDefinition -> AchievementCondition
        modelBuilder.Entity<AchievementConditionEntity>()
            .HasOne(e => e.AchievementDefinition)
            .WithMany(e => e.AchievementConditions)
            .HasForeignKey(e => e.AchievementId);
        // 1:N Relationship: AchievementDefinition -> AchievementReward
        modelBuilder.Entity<AchievementRewardEntity>()
            .HasOne(e => e.AchievementDefinition)
            .WithMany(e => e.AchievementRewards)
            .HasForeignKey(e => e.AchievementId);
        // 1:N Relationship: Player -> AchievementLog
        modelBuilder.Entity<AchievementLogEntity>()
            .HasOne(e => e.Player)
            .WithMany(e => e.AchievementLogs)
            .HasForeignKey(e => e.PlayerId);
        // 1:N Relationship: AchievementDefinition -> AchievementLog
        modelBuilder.Entity<AchievementLogEntity>()
            .HasOne(e => e.AchievementDefinition)
            .WithMany(e => e.AchievementLogs)
            .HasForeignKey(e => e.AchievementId);
        // 1:N Relationship: AchievementLog -> AchievementConditionProgress
        modelBuilder.Entity<AchievementConditionProgressEntity>()
            .HasOne(e => e.AchievementLog)
            .WithMany(e => e.AchievementConditionProgresses)
            .HasForeignKey(e => e.AchievementLogId);
        // 1:N Relationship: AchievementCondition -> AchievementConditionProgress
        modelBuilder.Entity<AchievementConditionProgressEntity>()
            .HasOne(e => e.AchievementCondition)
            .WithMany(e => e.AchievementConditionProgresses)
            .HasForeignKey(e => e.AchievementConditionId);
        // 1:N Relationship: AchievementLog -> AchievementRewardLog
        modelBuilder.Entity<AchievementRewardLogEntity>()
            .HasOne(e => e.AchievementLog)
            .WithMany(e => e.AchievementRewardLogs)
            .HasForeignKey(e => e.AchievementLogId);
        // 1:N Relationship: AchievementReward -> AchievementRewardLog
        modelBuilder.Entity<AchievementRewardLogEntity>()
            .HasOne(e => e.AchievementReward)
            .WithMany(e => e.AchievementRewardLogs)
            .HasForeignKey(e => e.AchievementRewardId);
        // Self-referencing: MapDefinition
        modelBuilder.Entity<MapDefinitionEntity>()
            .HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentId);
        // Self-referencing: ClassDefinition
        modelBuilder.Entity<ClassDefinitionEntity>()
            .HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentId);
        // Self-referencing: PetDefinition
        modelBuilder.Entity<PetDefinitionEntity>()
            .HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // 디자인 타임 또는 기본 구성용 연결 문자열 (환경 변수 등 사용 권장)
            string? connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                // 기본값 또는 오류 처리 (예: 개발용 로컬 DB)
                connectionString = "Host=localhost;Port=5432;Database=wiw_db;Username=user;Password=password;"; // 예시입니다. 실제 값으로 변경하세요.
                Console.WriteLine("[WARN] POSTGRES_CONNECTION_STRING environment variable not set. Using default connection string.");
            }
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
