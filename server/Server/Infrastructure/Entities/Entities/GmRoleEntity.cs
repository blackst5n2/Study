using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("GMRole")]
    public class GmRoleEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("role_type")]
        public GmRoleType RoleType { get; set; }
        [Column("permissions")]
        public string Permissions { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GMAccount_gm_role_id </summary>
        public virtual ICollection<GmAccountEntity> GmAccounts { get; set; } = new HashSet<GmAccountEntity>();
        #endregion
    }
}