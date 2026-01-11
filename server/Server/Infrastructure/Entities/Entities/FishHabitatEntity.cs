using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("FishHabitat")]
    public class FishHabitatEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("fish_definition_id")]
        public Guid FishDefinitionId { get; set; }
        [Column("fishing_spot_id")]
        public Guid FishingSpotId { get; set; }
        [Column("spawn_chance")]
        public float SpawnChance { get; set; }
        [Column("required_bait_item_id")]
        public Guid? RequiredBaitItemId { get; set; }
        [Column("required_time")]
        public TimeOfDayType RequiredTime { get; set; }
        [Column("required_season")]
        public SeasonType RequiredSeason { get; set; }
        [Column("required_weather")]
        public WeatherType RequiredWeather { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_FishHabitat_fish_definition_id </summary>
        public virtual FishDefinitionEntity FishDefinition { get; set; }
        public Guid FishingSpotDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_FishHabitat_fishing_spot_id </summary>
        public virtual FishingSpotDefinitionEntity FishingSpotDefinition { get; set; }
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_FishHabitat_required_bait_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}