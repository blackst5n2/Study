using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("FishDefinition")]
    public class FishDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("entity_definition_id")]
        public Guid EntityDefinitionId { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("grade")]
        public ItemGrade Grade { get; set; }
        [Column("required_fishing_level")]
        public int RequiredFishingLevel { get; set; }
        [Column("item_definition_id")]
        public Guid ItemDefinitionId { get; set; }
        [Column("min_length_cm")]
        public float? MinLengthCm { get; set; }
        [Column("max_length_cm")]
        public float? MaxLengthCm { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_FishDefinition_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_FishHabitat_fish_definition_id </summary>
        public virtual ICollection<FishHabitatEntity> FishHabitats { get; set; } = new HashSet<FishHabitatEntity>();
        /// <summary> Relation Label: FK_PlayerFishingLog_fish_definition_id </summary>
        public virtual ICollection<PlayerFishingLogEntity> PlayerFishingLogs { get; set; } = new HashSet<PlayerFishingLogEntity>();
        /// <summary> Relation Label: FK_FishDefinition_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}