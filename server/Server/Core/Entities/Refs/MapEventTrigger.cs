using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Refs
{
    public class MapEventTrigger
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public WorldEventType EventType { get; set; }
        public float? PosX { get; set; }
        public float? PosY { get; set; }
        public float? PosZ { get; set; }
        public float? TriggerRadius { get; set; }
        public string TriggerCondition { get; set; }
        public string Action { get; set; }
        public string Parameters { get; set; }
        public int? CooldownSeconds { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapEventTrigger_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        #endregion
    }
}