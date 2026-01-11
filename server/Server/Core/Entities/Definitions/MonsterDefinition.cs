using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class MonsterDefinition
    {
        public Guid Id { get; set; }
        public Guid EntityDefinitionId { get; set; }
        public int Level { get; set; }
        public MonsterType Type { get; set; }
        public int ExpReward { get; set; }
        public Guid? FsmId { get; set; }
        public Guid? BtId { get; set; }
        public string Description { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MonsterDefinition_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_MonsterDefinition_fsm_id </summary>
        public virtual MonsterAiFsmDefinition MonsterAiFsmDefinition { get; set; }
        /// <summary> Relation Label: FK_MonsterDefinition_bt_id </summary>
        public virtual MonsterAiBtDefinition MonsterAiBtDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerMonsterKillLog_monster_definition_id </summary>
        public virtual ICollection<PlayerMonsterKillLog> PlayerMonsterKillLogs { get; set; } = new HashSet<PlayerMonsterKillLog>();
        #endregion
    }
}