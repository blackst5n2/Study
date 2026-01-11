using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("GuildDefinition")]
    public class GuildDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("tag")]
        public string Tag { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("leader_id")]
        public Guid LeaderId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("experience")]
        public int Experience { get; set; }
        [Column("member_limit")]
        public int MemberLimit { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("is_recruiting")]
        public bool IsRecruiting { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ChatLog_guild_id </summary>
        public virtual ICollection<ChatLogEntity> ChatLogs { get; set; } = new HashSet<ChatLogEntity>();
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_GuildDefinition_leader_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_GuildRole_guild_id </summary>
        public virtual ICollection<GuildRoleEntity> GuildRoles { get; set; } = new HashSet<GuildRoleEntity>();
        /// <summary> Relation Label: FK_GuildMember_guild_id </summary>
        public virtual ICollection<GuildMemberEntity> GuildMembers { get; set; } = new HashSet<GuildMemberEntity>();
        /// <summary> Relation Label: FK_GuildNotice_guild_id </summary>
        public virtual ICollection<GuildNoticeEntity> GuildNotices { get; set; } = new HashSet<GuildNoticeEntity>();
        /// <summary> Relation Label: FK_GuildJoinRequest_guild_id </summary>
        public virtual ICollection<GuildJoinRequestEntity> GuildJoinRequests { get; set; } = new HashSet<GuildJoinRequestEntity>();
        /// <summary> Relation Label: FK_GuildContributionLog_guild_id </summary>
        public virtual ICollection<GuildContributionLogEntity> GuildContributionLogs { get; set; } = new HashSet<GuildContributionLogEntity>();
        /// <summary> Relation Label: FK_GuildTitle_guild_id </summary>
        public virtual ICollection<GuildTitleEntity> GuildTitles { get; set; } = new HashSet<GuildTitleEntity>();
        #endregion
    }
}