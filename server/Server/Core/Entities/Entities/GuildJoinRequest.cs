using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class GuildJoinRequest
    {
        public Guid Id { get; set; }
        public Guid GuildId { get; set; }
        public Guid PlayerId { get; set; }
        public GuildJoinRequestStatus Status { get; set; }
        public string Message { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public Guid? ProcessedBy { get; set; }
        public string Reason { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GuildJoinRequest_guild_id </summary>
        public virtual GuildDefinition GuildDefinition { get; set; }
        /// <summary> Relation Label: FK_GuildJoinRequest_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}