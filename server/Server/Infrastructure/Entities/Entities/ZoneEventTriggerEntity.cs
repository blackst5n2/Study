using Server.Enums;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("ZoneEventTrigger")]
    public class ZoneEventTriggerEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("zone_id")]
        public Guid ZoneId { get; set; }
        [Column("event_type")]
        public WorldEventType EventType { get; set; }
        [Column("trigger_condition")]
        public string TriggerCondition { get; set; }
        [Column("action")]
        public string Action { get; set; }
        [Column("parameters")]
        public string Parameters { get; set; }
        [Column("priority")]
        public int Priority { get; set; }
        [Column("cooldown_seconds")]
        public int? CooldownSeconds { get; set; }
        [Column("max_triggers")]
        public int? MaxTriggers { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapZoneId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ZoneEventTrigger_zone_id </summary>
        public virtual MapZoneEntity MapZone { get; set; }
        #endregion
    }
}