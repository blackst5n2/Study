using Server.Core.Entities.Details;
using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Core.Entities.Refs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class ItemDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public ItemSubType SubType { get; set; }
        public ItemGrade Grade { get; set; }
        public ItemRarity Rarity { get; set; }
        public EquipSlot EquipSlot { get; set; }
        public int MaxStack { get; set; }
        public int? BaseValue { get; set; }
        public int? Price { get; set; }
        public bool IsTradable { get; set; }
        public bool IsUsable { get; set; }
        public Guid? UseEffectId { get; set; }
        public int? RequiredLevel { get; set; }
        public string Icon { get; set; }
        public string ModelAsset { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DropTableDetail_item_definition_id </summary>
        public virtual ICollection<DropTableDetail> DropTableDetails { get; set; } = new HashSet<DropTableDetail>();
        /// <summary> Relation Label: FK_ItemDefinition_use_effect_id </summary>
        public virtual ItemEffectDefinition ItemEffectDefinition { get; set; }
        /// <summary> Relation Label: FK_ItemInstance_item_definition_id </summary>
        public virtual ICollection<ItemInstance> ItemInstances { get; set; } = new HashSet<ItemInstance>();
        /// <summary> Relation Label: FK_ItemAcquisitionLog_item_definition_id </summary>
        public virtual ICollection<ItemAcquisitionLog> ItemAcquisitionLogs { get; set; } = new HashSet<ItemAcquisitionLog>();
        /// <summary> Relation Label: FK_ItemEnhancementLog_item_definition_id </summary>
        public virtual ICollection<ItemEnhancementLog> ItemEnhancementLogs { get; set; } = new HashSet<ItemEnhancementLog>();
        /// <summary> Relation Label: FK_RecipeIngredient_item_definition_id </summary>
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new HashSet<RecipeIngredient>();
        /// <summary> Relation Label: FK_RecipeResult_item_definition_id </summary>
        public virtual ICollection<RecipeResult> RecipeResults { get; set; } = new HashSet<RecipeResult>();
        /// <summary> Relation Label: FK_PlayerCraftingLog_result_item_definition_id </summary>
        public virtual ICollection<PlayerCraftingLog> PlayerCraftingLogs { get; set; } = new HashSet<PlayerCraftingLog>();
        /// <summary> Relation Label: FK_RecipeDefinition_fail_reward_item_id </summary>
        public virtual ICollection<RecipeDefinition> RecipeDefinitions { get; set; } = new HashSet<RecipeDefinition>();
        /// <summary> Relation Label: FK_CookingResultDefinition_result_item_id </summary>
        public virtual ICollection<CookingResultDefinition> CookingResultDefinitions { get; set; } = new HashSet<CookingResultDefinition>();
        /// <summary> Relation Label: FK_CookingGradeRewardDefinition_reward_item_id </summary>
        public virtual ICollection<CookingGradeRewardDefinition> CookingGradeRewardDefinitions { get; set; } = new HashSet<CookingGradeRewardDefinition>();
        /// <summary> Relation Label: FK_SeedGradeToCropGradeMap_seed_item_id </summary>
        public virtual ICollection<SeedGradeToCropGradeMap> SeedGradeToCropGradeMaps { get; set; } = new HashSet<SeedGradeToCropGradeMap>();
        /// <summary> Relation Label: FK_CropProduct_item_definition_id </summary>
        public virtual ICollection<CropProduct> CropProducts { get; set; } = new HashSet<CropProduct>();
        /// <summary> Relation Label: FK_FishHabitat_required_bait_item_id </summary>
        public virtual ICollection<FishHabitat> FishHabitats { get; set; } = new HashSet<FishHabitat>();
        /// <summary> Relation Label: FK_PlayerFishingLog_bait_item_id </summary>
        public virtual ICollection<PlayerFishingLog> PlayerFishingLogs { get; set; } = new HashSet<PlayerFishingLog>();
        /// <summary> Relation Label: FK_FishDefinition_item_definition_id </summary>
        public virtual ICollection<FishDefinition> FishDefinitions { get; set; } = new HashSet<FishDefinition>();
        /// <summary> Relation Label: FK_LivestockDefinition_product_item_id </summary>
        public virtual ICollection<LivestockDefinition> LivestockDefinitions { get; set; } = new HashSet<LivestockDefinition>();
        /// <summary> Relation Label: FK_PlayerLivestockProductLog_item_definition_id </summary>
        public virtual ICollection<PlayerLivestockProductLog> PlayerLivestockProductLogs { get; set; } = new HashSet<PlayerLivestockProductLog>();
        /// <summary> Relation Label: FK_PlayerLivestockFeedLog_item_definition_id </summary>
        public virtual ICollection<PlayerLivestockFeedLog> PlayerLivestockFeedLogs { get; set; } = new HashSet<PlayerLivestockFeedLog>();
        /// <summary> Relation Label: FK_PlayerMiningLog_tool_item_id </summary>
        public virtual ICollection<PlayerMiningLog> PlayerMiningLogs { get; set; } = new HashSet<PlayerMiningLog>();
        /// <summary> Relation Label: FK_PlayerLoggingLog_tool_item_id </summary>
        public virtual ICollection<PlayerLoggingLog> PlayerLoggingLogs { get; set; } = new HashSet<PlayerLoggingLog>();
        /// <summary> Relation Label: FK_MiniGameRewardDefinition_reward_item_id </summary>
        public virtual ICollection<MiniGameRewardDefinition> MiniGameRewardDefinitions { get; set; } = new HashSet<MiniGameRewardDefinition>();
        /// <summary> Relation Label: FK_PlayerMiniGameResult_reward_item_id </summary>
        public virtual ICollection<PlayerMiniGameResult> PlayerMiniGameResults { get; set; } = new HashSet<PlayerMiniGameResult>();
        /// <summary> Relation Label: FK_JobSkillLevelDefinition_reward_item_id </summary>
        public virtual ICollection<JobSkillLevelDefinition> JobSkillLevelDefinitions { get; set; } = new HashSet<JobSkillLevelDefinition>();
        /// <summary> Relation Label: FK_JobLevelDefinition_reward_item_id </summary>
        public virtual ICollection<JobLevelDefinition> JobLevelDefinitions { get; set; } = new HashSet<JobLevelDefinition>();
        /// <summary> Relation Label: FK_PlayerQuickSlot_item_definition_id </summary>
        public virtual ICollection<PlayerQuickSlot> PlayerQuickSlots { get; set; } = new HashSet<PlayerQuickSlot>();
        /// <summary> Relation Label: FK_MailAttachment_item_definition_id </summary>
        public virtual ICollection<MailAttachment> MailAttachments { get; set; } = new HashSet<MailAttachment>();
        /// <summary> Relation Label: FK_MarketListing_item_definition_id </summary>
        public virtual ICollection<MarketListing> MarketListings { get; set; } = new HashSet<MarketListing>();
        /// <summary> Relation Label: FK_MarketTransaction_item_definition_id </summary>
        public virtual ICollection<MarketTransaction> MarketTransactions { get; set; } = new HashSet<MarketTransaction>();
        /// <summary> Relation Label: FK_ShopItemDefinition_item_definition_id </summary>
        public virtual ICollection<ShopItemDefinition> ShopItemDefinitions { get; set; } = new HashSet<ShopItemDefinition>();
        /// <summary> Relation Label: FK_ShopTransactionLog_item_definition_id </summary>
        public virtual ICollection<ShopTransactionLog> ShopTransactionLogs { get; set; } = new HashSet<ShopTransactionLog>();
        /// <summary> Relation Label: N:M via ItemTag </summary>
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        #endregion
    }
}