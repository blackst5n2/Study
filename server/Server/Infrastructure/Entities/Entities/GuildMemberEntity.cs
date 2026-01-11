using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("GuildMember")]
    public class GuildMemberEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("guild_id")]
        public Guid GuildId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("role_id")]
        public Guid RoleId { get; set; }
        [Column("joined_at")]
        public DateTime JoinedAt { get; set; }
        [Column("left_at")]
        public DateTime? LeftAt { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("contribution_points")]
        public int ContributionPoints { get; set; }
        [Column("last_activity_at")]
        public DateTime? LastActivityAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid GuildDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_GuildMember_guild_id </summary>
        public virtual GuildDefinitionEntity GuildDefinition { get; set; }
        /// <summary> Relation Label: FK_GuildMember_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid GuildRoleId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_GuildMember_role_id </summary>
        public virtual GuildRoleEntity GuildRole { get; set; }
        #endregion
    }
}