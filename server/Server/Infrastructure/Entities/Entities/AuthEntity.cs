using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("Auth")]
    public class AuthEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("account_id")]
        public Guid AccountId { get; set; }
        [Column("provider")]
        public AuthProvider Provider { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("oauth_id")]
        public string OauthId { get; set; }
        [Column("is_verified")]
        public bool IsVerified { get; set; }
        [Column("verify_token")]
        public string VerifyToken { get; set; }
        [Column("last_login_at")]
        public DateTime LastLoginAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Auth_account_id </summary>
        public virtual AccountEntity Account { get; set; }
        #endregion
    }
}