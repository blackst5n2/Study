using Server.Enums;
using Server.Infrastructure.Entities.Details;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("ItemDefinition")]
    public class ItemDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("type")]
        public ItemType Type { get; set; }
        [Column("sub_type")]
        public ItemSubType SubType { get; set; }
        [Column("grade")]
        public ItemGrade Grade { get; set; }
        [Column("rarity")]
        public ItemRarity Rarity { get; set; }
        [Column("equip_slot")]
        public EquipSlot EquipSlot { get; set; }
        [Column("max_stack")]
        public int MaxStack { get; set; }
        [Column("base_value")]
        public int? BaseValue { get; set; }
        [Column("price")]
        public int? Price { get; set; }
        [Column("is_tradable")]
        public bool IsTradable { get; set; }
        [Column("is_usable")]
        public bool IsUsable { get; set; }
        [Column("use_effect_id")]
        public Guid? UseEffectId { get; set; }
        [Column("required_level")]
        public int? RequiredLevel { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("model_asset")]
        public string ModelAsset { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DropTableDetail_item_definition_id </summary>
        public virtual ICollection<DropTableDetailEntity> DropTableDetails { get; set; } = new HashSet<DropTableDetailEntity>();
        public Guid ItemEffectDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ItemDefinition_use_effect_id </summary>
        public virtual ItemEffectDefinitionEntity ItemEffectDefinition { get; set; }
        /// <summary> Relation Label: FK_ItemInstance_item_definition_id </summary>
        public virtual ICollection<ItemInstanceEntity> ItemInstances { get; set; } = new HashSet<ItemInstanceEntity>();
        /// <summary> Relation Label: FK_ItemAcquisitionLog_item_definition_id </summary>
        public virtual ICollection<ItemAcquisitionLogEntity> ItemAcquisitionLogs { get; set; } = new HashSet<ItemAcquisitionLogEntity>();
        /// <summary> Relation Label: FK_ItemEnhancementLog_item_definition_id </summary>
        public virtual ICollection<ItemEnhancementLogEntity> ItemEnhancementLogs { get; set; } = new HashSet<ItemEnhancementLogEntity>();
        /// <summary> Relation Label: FK_RecipeIngredient_item_definition_id </summary>
        public virtual ICollection<RecipeIngredientEntity> RecipeIngredients { get; set; } = new HashSet<RecipeIngredientEntity>();
        /// <summary> Relation Label: FK_RecipeResult_item_definition_id </summary>
        public virtual ICollection<RecipeResultEntity> RecipeResults { get; set; } = new HashSet<RecipeResultEntity>();
        /// <summary> Relation Label: FK_PlayerCraftingLog_result_item_definition_id </summary>
        public virtual ICollection<PlayerCraftingLogEntity> PlayerCraftingLogs { get; set; } = new HashSet<PlayerCraftingLogEntity>();
        /// <summary> Relation Label: FK_RecipeDefinition_fail_reward_item_id </summary>
        public virtual ICollection<RecipeDefinitionEntity> RecipeDefinitions { get; set; } = new HashSet<RecipeDefinitionEntity>();
        /// <summary> Relation Label: FK_CookingResultDefinition_result_item_id </summary>
        public virtual ICollection<CookingResultDefinitionEntity> CookingResultDefinitions { get; set; } = new HashSet<CookingResultDefinitionEntity>();
        /// <summary> Relation Label: FK_CookingGradeRewardDefinition_reward_item_id </summary>
        public virtual ICollection<CookingGradeRewardDefinitionEntity> CookingGradeRewardDefinitions { get; set; } = new HashSet<CookingGradeRewardDefinitionEntity>();
        /// <summary> Relation Label: FK_SeedGradeToCropGradeMap_seed_item_id </summary>
        public virtual ICollection<SeedGradeToCropGradeMapEntity> SeedGradeToCropGradeMaps { get; set; } = new HashSet<SeedGradeToCropGradeMapEntity>();
        /// <summary> Relation Label: FK_CropProduct_item_definition_id </summary>
        public virtual ICollection<CropProductEntity> CropProducts { get; set; } = new HashSet<CropProductEntity>();
        /// <summary> Relation Label: FK_FishHabitat_required_bait_item_id </summary>
        public virtual ICollection<FishHabitatEntity> FishHabitats { get; set; } = new HashSet<FishHabitatEntity>();
        /// <summary> Relation Label: FK_PlayerFishingLog_bait_item_id </summary>
        public virtual ICollection<PlayerFishingLogEntity> PlayerFishingLogs { get; set; } = new HashSet<PlayerFishingLogEntity>();
        /// <summary> Relation Label: FK_FishDefinition_item_definition_id </summary>
        public virtual ICollection<FishDefinitionEntity> FishDefinitions { get; set; } = new HashSet<FishDefinitionEntity>();
        /// <summary> Relation Label: FK_LivestockDefinition_product_item_id </summary>
        public virtual ICollection<LivestockDefinitionEntity> LivestockDefinitions { get; set; } = new HashSet<LivestockDefinitionEntity>();
        /// <summary> Relation Label: FK_PlayerLivestockProductLog_item_definition_id </summary>
        public virtual ICollection<PlayerLivestockProductLogEntity> PlayerLivestockProductLogs { get; set; } = new HashSet<PlayerLivestockProductLogEntity>();
        /// <summary> Relation Label: FK_PlayerLivestockFeedLog_item_definition_id </summary>
        public virtual ICollection<PlayerLivestockFeedLogEntity> PlayerLivestockFeedLogs { get; set; } = new HashSet<PlayerLivestockFeedLogEntity>();
        /// <summary> Relation Label: FK_PlayerMiningLog_tool_item_id </summary>
        public virtual ICollection<PlayerMiningLogEntity> PlayerMiningLogs { get; set; } = new HashSet<PlayerMiningLogEntity>();
        /// <summary> Relation Label: FK_PlayerLoggingLog_tool_item_id </summary>
        public virtual ICollection<PlayerLoggingLogEntity> PlayerLoggingLogs { get; set; } = new HashSet<PlayerLoggingLogEntity>();
        /// <summary> Relation Label: FK_MiniGameRewardDefinition_reward_item_id </summary>
        public virtual ICollection<MiniGameRewardDefinitionEntity> MiniGameRewardDefinitions { get; set; } = new HashSet<MiniGameRewardDefinitionEntity>();
        /// <summary> Relation Label: FK_PlayerMiniGameResult_reward_item_id </summary>
        public virtual ICollection<PlayerMiniGameResultEntity> PlayerMiniGameResults { get; set; } = new HashSet<PlayerMiniGameResultEntity>();
        /// <summary> Relation Label: FK_JobSkillLevelDefinition_reward_item_id </summary>
        public virtual ICollection<JobSkillLevelDefinitionEntity> JobSkillLevelDefinitions { get; set; } = new HashSet<JobSkillLevelDefinitionEntity>();
        /// <summary> Relation Label: FK_JobLevelDefinition_reward_item_id </summary>
        public virtual ICollection<JobLevelDefinitionEntity> JobLevelDefinitions { get; set; } = new HashSet<JobLevelDefinitionEntity>();
        /// <summary> Relation Label: FK_PlayerQuickSlot_item_definition_id </summary>
        public virtual ICollection<PlayerQuickSlotEntity> PlayerQuickSlots { get; set; } = new HashSet<PlayerQuickSlotEntity>();
        /// <summary> Relation Label: FK_MailAttachment_item_definition_id </summary>
        public virtual ICollection<MailAttachmentEntity> MailAttachments { get; set; } = new HashSet<MailAttachmentEntity>();
        /// <summary> Relation Label: FK_MarketListing_item_definition_id </summary>
        public virtual ICollection<MarketListingEntity> MarketListings { get; set; } = new HashSet<MarketListingEntity>();
        /// <summary> Relation Label: FK_MarketTransaction_item_definition_id </summary>
        public virtual ICollection<MarketTransactionEntity> MarketTransactions { get; set; } = new HashSet<MarketTransactionEntity>();
        /// <summary> Relation Label: FK_ShopItemDefinition_item_definition_id </summary>
        public virtual ICollection<ShopItemDefinitionEntity> ShopItemDefinitions { get; set; } = new HashSet<ShopItemDefinitionEntity>();
        /// <summary> Relation Label: FK_ShopTransactionLog_item_definition_id </summary>
        public virtual ICollection<ShopTransactionLogEntity> ShopTransactionLogs { get; set; } = new HashSet<ShopTransactionLogEntity>();
        /// <summary> Relation Label: N:M via ItemTag </summary>
        public virtual ICollection<TagEntity> Tags { get; set; } = new HashSet<TagEntity>();
        #endregion
    }
}