using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class GuildMember
    {
        public Guid Id { get; set; }
        public Guid GuildId { get; set; }
        public Guid PlayerId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public bool IsActive { get; set; }
        public int ContributionPoints { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GuildMember_guild_id </summary>
        public virtual GuildDefinition GuildDefinition { get; set; }
        /// <summary> Relation Label: FK_GuildMember_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_GuildMember_role_id </summary>
        public virtual GuildRole GuildRole { get; set; }
        #endregion
    }
}