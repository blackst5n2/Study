using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Definitions
{
    public class GuildDefinition
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public Guid LeaderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int MemberLimit { get; set; }
        public bool IsActive { get; set; }
        public bool IsRecruiting { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ChatLog_guild_id </summary>
        public virtual ICollection<ChatLog> ChatLogs { get; set; } = new HashSet<ChatLog>();
        /// <summary> Relation Label: FK_GuildDefinition_leader_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_GuildRole_guild_id </summary>
        public virtual ICollection<GuildRole> GuildRoles { get; set; } = new HashSet<GuildRole>();
        /// <summary> Relation Label: FK_GuildMember_guild_id </summary>
        public virtual ICollection<GuildMember> GuildMembers { get; set; } = new HashSet<GuildMember>();
        /// <summary> Relation Label: FK_GuildNotice_guild_id </summary>
        public virtual ICollection<GuildNotice> GuildNotices { get; set; } = new HashSet<GuildNotice>();
        /// <summary> Relation Label: FK_GuildJoinRequest_guild_id </summary>
        public virtual ICollection<GuildJoinRequest> GuildJoinRequests { get; set; } = new HashSet<GuildJoinRequest>();
        /// <summary> Relation Label: FK_GuildContributionLog_guild_id </summary>
        public virtual ICollection<GuildContributionLog> GuildContributionLogs { get; set; } = new HashSet<GuildContributionLog>();
        /// <summary> Relation Label: FK_GuildTitle_guild_id </summary>
        public virtual ICollection<GuildTitle> GuildTitles { get; set; } = new HashSet<GuildTitle>();
        #endregion
    }
}