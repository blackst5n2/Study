using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class WorldEvent
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WorldEventType EventType { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool IsActive { get; set; }
        public string TriggerCondition { get; set; }
        public string EffectData { get; set; }
        public Guid? MapId { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerWorldEventParticipation_event_id </summary>
        public virtual ICollection<PlayerWorldEventParticipation> PlayerWorldEventParticipations { get; set; } = new HashSet<PlayerWorldEventParticipation>();
        /// <summary> Relation Label: FK_WorldEvent_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        #endregion
    }
}