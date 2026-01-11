using Server.Core.Entities.Progress;

namespace Server.Core.Entities.UserContents
{
    public class UserPatternComment
    {
        public Guid Id { get; set; }
        public Guid PatternId { get; set; }
        public Guid PlayerId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserPatternComment_pattern_id </summary>
        public virtual UserPattern UserPattern { get; set; }
        /// <summary> Relation Label: FK_UserPatternComment_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}