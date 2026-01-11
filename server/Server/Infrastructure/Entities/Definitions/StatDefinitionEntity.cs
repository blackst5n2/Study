using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("StatDefinition")]
    public class StatDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("stat_type")]
        public StatType StatType { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("is_percentage")]
        public bool IsPercentage { get; set; }
        [Column("default_value")]
        public long DefaultValue { get; set; }
        [Column("min_value")]
        public long? MinValue { get; set; }
        [Column("max_value")]
        public long? MaxValue { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_StatValue_stat_definition_id </summary>
        public virtual ICollection<StatValueEntity> StatValues { get; set; } = new HashSet<StatValueEntity>();
        /// <summary> Relation Label: FK_PlayerStatLog_stat_definition_id </summary>
        public virtual ICollection<PlayerStatLogEntity> PlayerStatLogs { get; set; } = new HashSet<PlayerStatLogEntity>();
        #endregion
    }
}