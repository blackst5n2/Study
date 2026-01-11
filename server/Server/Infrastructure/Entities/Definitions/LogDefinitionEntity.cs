using Server.Enums;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("LogDefinition")]
    public class LogDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("entity_definition_id")]
        public Guid EntityDefinitionId { get; set; }
        [Column("grade")]
        public ItemGrade Grade { get; set; }
        [Column("required_tool_subtype")]
        public ItemSubType RequiredToolSubtype { get; set; }
        [Column("required_tool_level")]
        public int RequiredToolLevel { get; set; }
        [Column("required_skill_level")]
        public int RequiredSkillLevel { get; set; }
        [Column("logging_difficulty")]
        public int LoggingDifficulty { get; set; }
        [Column("respawn_time_seconds")]
        public int? RespawnTimeSeconds { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_LogDefinition_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerLoggingLog_log_definition_id </summary>
        public virtual ICollection<PlayerLoggingLogEntity> PlayerLoggingLogs { get; set; } = new HashSet<PlayerLoggingLogEntity>();
        #endregion
    }
}