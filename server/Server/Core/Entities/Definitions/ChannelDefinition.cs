using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class ChannelDefinition
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public ChannelType Type { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ChatLog_channel_code </summary>
        public virtual ICollection<ChatLog> ChatLogs { get; set; } = new HashSet<ChatLog>();
        #endregion
    }
}