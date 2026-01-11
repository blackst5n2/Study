using Server.Enums;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("ChannelDefinition")]
    public class ChannelDefinitionEntity
    {
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("type")]
        public ChannelType Type { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ChatLog_channel_code </summary>
        public virtual ICollection<ChatLogEntity> ChatLogs { get; set; } = new HashSet<ChatLogEntity>();
        #endregion
    }
}