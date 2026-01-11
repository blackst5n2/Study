using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class GmRole
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public GmRoleType RoleType { get; set; }
        public string Permissions { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GMAccount_gm_role_id </summary>
        public virtual ICollection<GmAccount> GmAccounts { get; set; } = new HashSet<GmAccount>();
        #endregion
    }
}