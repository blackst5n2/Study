using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("WorldEvent")]
    public class WorldEventEntity
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
        [Column("event_type")]
        public WorldEventType EventType { get; set; }
        [Column("start_at")]
        public DateTime StartAt { get; set; }
        [Column("end_at")]
        public DateTime EndAt { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("trigger_condition")]
        public string TriggerCondition { get; set; }
        [Column("effect_data")]
        public string EffectData { get; set; }
        [Column("map_id")]
        public Guid? MapId { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerWorldEventParticipation_event_id </summary>
        public virtual ICollection<PlayerWorldEventParticipationEntity> PlayerWorldEventParticipations { get; set; } = new HashSet<PlayerWorldEventParticipationEntity>();
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_WorldEvent_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        #endregion
    }
}