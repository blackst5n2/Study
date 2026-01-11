using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("MapEventTrigger")]
    public class MapEventTriggerEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("event_type")]
        public WorldEventType EventType { get; set; }
        [Column("pos_x")]
        public float? PosX { get; set; }
        [Column("pos_y")]
        public float? PosY { get; set; }
        [Column("pos_z")]
        public float? PosZ { get; set; }
        [Column("trigger_radius")]
        public float? TriggerRadius { get; set; }
        [Column("trigger_condition")]
        public string TriggerCondition { get; set; }
        [Column("action")]
        public string Action { get; set; }
        [Column("parameters")]
        public string Parameters { get; set; }
        [Column("cooldown_seconds")]
        public int? CooldownSeconds { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapEventTrigger_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        #endregion
    }
}