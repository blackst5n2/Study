using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("EventDefinition")]
    public class EventDefinitionEntity
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
        public EventType EventType { get; set; }
        [Column("receiver_type")]
        public EventReceiverType ReceiverType { get; set; }
        [Column("receiver_id")]
        public Guid? ReceiverId { get; set; }
        [Column("start_at")]
        public DateTime StartAt { get; set; }
        [Column("end_at")]
        public DateTime EndAt { get; set; }
        [Column("config")]
        public string Config { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerEventProgress_event_id </summary>
        public virtual ICollection<PlayerEventProgressEntity> PlayerEventProgresses { get; set; } = new HashSet<PlayerEventProgressEntity>();
        /// <summary> Relation Label: FK_EventReward_event_id </summary>
        public virtual ICollection<EventRewardEntity> EventRewards { get; set; } = new HashSet<EventRewardEntity>();
        #endregion
    }
}