using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class CommunityNotice
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NoticeType Type { get; set; }
        public Guid? AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public bool IsActive { get; set; }
        public string TargetChannel { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_CommunityNotice_author_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}