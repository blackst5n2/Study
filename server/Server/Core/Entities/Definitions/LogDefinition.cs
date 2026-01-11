using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class LogDefinition
    {
        public Guid Id { get; set; }
        public Guid EntityDefinitionId { get; set; }
        public ItemGrade Grade { get; set; }
        public ItemSubType RequiredToolSubtype { get; set; }
        public int RequiredToolLevel { get; set; }
        public int RequiredSkillLevel { get; set; }
        public int LoggingDifficulty { get; set; }
        public int? RespawnTimeSeconds { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_LogDefinition_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerLoggingLog_log_definition_id </summary>
        public virtual ICollection<PlayerLoggingLog> PlayerLoggingLogs { get; set; } = new HashSet<PlayerLoggingLog>();
        #endregion
    }
}