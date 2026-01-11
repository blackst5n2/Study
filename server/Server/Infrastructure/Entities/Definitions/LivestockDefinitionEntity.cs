using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("LivestockDefinition")]
    public class LivestockDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("entity_definition_id")]
        public Guid EntityDefinitionId { get; set; }
        [Column("grade")]
        public ItemGrade Grade { get; set; }
        [Column("growth_stages")]
        public int GrowthStages { get; set; }
        [Column("growth_time_minutes_per_stage")]
        public int GrowthTimeMinutesPerStage { get; set; }
        [Column("feed_tag")]
        public string FeedTag { get; set; }
        [Column("required_nutrition_per_day")]
        public int RequiredNutritionPerDay { get; set; }
        [Column("disease_chance")]
        public float DiseaseChance { get; set; }
        [Column("product_interval_minutes")]
        public int? ProductIntervalMinutes { get; set; }
        [Column("product_item_id")]
        public Guid? ProductItemId { get; set; }
        [Column("min_product_amount")]
        public int MinProductAmount { get; set; }
        [Column("max_product_amount")]
        public int MaxProductAmount { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_LivestockDefinition_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_LivestockDefinition_product_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerLivestockInstance_livestock_definition_id </summary>
        public virtual ICollection<PlayerLivestockInstanceEntity> PlayerLivestockInstances { get; set; } = new HashSet<PlayerLivestockInstanceEntity>();
        /// <summary> Relation Label: N:M via LivestockAllowedFeed </summary>
        public virtual ICollection<ItemDefinitionEntity> ItemDefinitions { get; set; } = new HashSet<ItemDefinitionEntity>();
        #endregion
    }
}