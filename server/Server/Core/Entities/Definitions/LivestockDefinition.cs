using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class LivestockDefinition
    {
        public Guid Id { get; set; }
        public Guid EntityDefinitionId { get; set; }
        public ItemGrade Grade { get; set; }
        public int GrowthStages { get; set; }
        public int GrowthTimeMinutesPerStage { get; set; }
        public string FeedTag { get; set; }
        public int RequiredNutritionPerDay { get; set; }
        public float DiseaseChance { get; set; }
        public int? ProductIntervalMinutes { get; set; }
        public Guid? ProductItemId { get; set; }
        public int MinProductAmount { get; set; }
        public int MaxProductAmount { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_LivestockDefinition_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_LivestockDefinition_product_item_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerLivestockInstance_livestock_definition_id </summary>
        public virtual ICollection<PlayerLivestockInstance> PlayerLivestockInstances { get; set; } = new HashSet<PlayerLivestockInstance>();
        /// <summary> Relation Label: N:M via LivestockAllowedFeed </summary>
        public virtual ICollection<ItemDefinition> ItemDefinitions { get; set; } = new HashSet<ItemDefinition>();
        #endregion
    }
}