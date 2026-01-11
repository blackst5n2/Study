using Server.Core.Entities.Logs;

namespace Server.Core.Entities.Definitions
{
    public class LevelDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int Level { get; set; }
        public long ExpRequired { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerLevelLog_level_code </summary>
        public virtual ICollection<PlayerLevelLog> PlayerLevelLogs { get; set; } = new HashSet<PlayerLevelLog>();
        #endregion
    }
}