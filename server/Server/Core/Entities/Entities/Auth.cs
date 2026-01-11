using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class Auth
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public AuthProvider Provider { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string OauthId { get; set; }
        public bool IsVerified { get; set; }
        public string VerifyToken { get; set; }
        public DateTime LastLoginAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Auth_account_id </summary>
        public virtual Account Account { get; set; }
        #endregion
    }
}