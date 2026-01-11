using Server.Core.Entities.Refs;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class ZoneEventTrigger
    {
        public Guid Id { get; set; }
        public Guid ZoneId { get; set; }
        public WorldEventType EventType { get; set; }
        public string TriggerCondition { get; set; }
        public string Action { get; set; }
        public string Parameters { get; set; }
        public int Priority { get; set; }
        public int? CooldownSeconds { get; set; }
        public int? MaxTriggers { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ZoneEventTrigger_zone_id </summary>
        public virtual MapZone MapZone { get; set; }
        #endregion
    }
}