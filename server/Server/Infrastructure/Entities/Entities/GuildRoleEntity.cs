using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("GuildRole")]
    public class GuildRoleEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("guild_id")]
        public Guid GuildId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("is_leader_role")]
        public bool IsLeaderRole { get; set; }
        [Column("permissions")]
        public string Permissions { get; set; }
        [Column("role_order")]
        public int RoleOrder { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid GuildDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_GuildRole_guild_id </summary>
        public virtual GuildDefinitionEntity GuildDefinition { get; set; }
        /// <summary> Relation Label: FK_GuildMember_role_id </summary>
        public virtual ICollection<GuildMemberEntity> GuildMembers { get; set; } = new HashSet<GuildMemberEntity>();
        #endregion
    }
}