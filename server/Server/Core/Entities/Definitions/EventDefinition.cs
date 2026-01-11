using Server.Core.Entities.Entities;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class EventDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EventType EventType { get; set; }
        public EventReceiverType ReceiverType { get; set; }
        public Guid? ReceiverId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Config { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerEventProgress_event_id </summary>
        public virtual ICollection<PlayerEventProgress> PlayerEventProgresses { get; set; } = new HashSet<PlayerEventProgress>();
        /// <summary> Relation Label: FK_EventReward_event_id </summary>
        public virtual ICollection<EventReward> EventRewards { get; set; } = new HashSet<EventReward>();
        #endregion
    }
}