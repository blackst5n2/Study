using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class GmNotice
    {
        public Guid Id { get; set; }
        public Guid GmAccountId { get; set; }
        public GmNoticeType NoticeType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public bool IsActive { get; set; }
        public string TargetAudience { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GMNotice_gm_account_id </summary>
        public virtual GmAccount GmAccount { get; set; }
        #endregion
    }
}