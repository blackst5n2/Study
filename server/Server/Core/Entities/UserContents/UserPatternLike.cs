using Server.Core.Entities.Progress;

namespace Server.Core.Entities.UserContents
{
    public class UserPatternLike
    {
        public Guid PatternId { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime LikedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserPatternLike_pattern_id </summary>
        public virtual UserPattern UserPattern { get; set; }
        /// <summary> Relation Label: FK_UserPatternLike_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}