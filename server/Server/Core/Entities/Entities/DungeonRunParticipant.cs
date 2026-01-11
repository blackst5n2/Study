using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class DungeonRunParticipant
    {
        public Guid Id { get; set; }
        public Guid DungeonRunId { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public bool IsCleared { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DungeonRunParticipant_dungeon_run_id </summary>
        public virtual DungeonRun DungeonRun { get; set; }
        /// <summary> Relation Label: FK_DungeonRunParticipant_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}