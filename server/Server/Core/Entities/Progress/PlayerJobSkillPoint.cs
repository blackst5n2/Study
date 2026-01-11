using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerJobSkillPoint
    {
        public Guid PlayerId { get; set; }
        public Guid JobId { get; set; }
        public int TotalPoints { get; set; }
        public int UsedPoints { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerJobSkillPoint_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerJobSkillPoint_job_id </summary>
        public virtual JobDefinition JobDefinition { get; set; }
        #endregion
    }
}