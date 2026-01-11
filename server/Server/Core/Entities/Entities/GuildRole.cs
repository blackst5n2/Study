using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class GuildRole
    {
        public Guid Id { get; set; }
        public Guid GuildId { get; set; }
        public string Name { get; set; }
        public bool IsLeaderRole { get; set; }
        public string Permissions { get; set; }
        public int RoleOrder { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GuildRole_guild_id </summary>
        public virtual GuildDefinition GuildDefinition { get; set; }
        /// <summary> Relation Label: FK_GuildMember_role_id </summary>
        public virtual ICollection<GuildMember> GuildMembers { get; set; } = new HashSet<GuildMember>();
        #endregion
    }
}