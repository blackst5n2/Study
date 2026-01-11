using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class StatDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public StatType StatType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPercentage { get; set; }
        public long DefaultValue { get; set; }
        public long? MinValue { get; set; }
        public long? MaxValue { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_StatValue_stat_definition_id </summary>
        public virtual ICollection<StatValue> StatValues { get; set; } = new HashSet<StatValue>();
        /// <summary> Relation Label: FK_PlayerStatLog_stat_definition_id </summary>
        public virtual ICollection<PlayerStatLog> PlayerStatLogs { get; set; } = new HashSet<PlayerStatLog>();
        #endregion
    }
}