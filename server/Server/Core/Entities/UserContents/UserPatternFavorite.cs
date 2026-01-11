using Server.Core.Entities.Progress;

namespace Server.Core.Entities.UserContents
{
    public class UserPatternFavorite
    {
        public Guid PlayerId { get; set; }
        public Guid PatternId { get; set; }
        public DateTime CreatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserPatternFavorite_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_UserPatternFavorite_pattern_id </summary>
        public virtual UserPattern UserPattern { get; set; }
        #endregion
    }
}