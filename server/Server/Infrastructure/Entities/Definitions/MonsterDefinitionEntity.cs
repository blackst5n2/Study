using Server.Enums;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("MonsterDefinition")]
    public class MonsterDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("entity_definition_id")]
        public Guid EntityDefinitionId { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("type")]
        public MonsterType Type { get; set; }
        [Column("exp_reward")]
        public int ExpReward { get; set; }
        [Column("fsm_id")]
        public Guid? FsmId { get; set; }
        [Column("bt_id")]
        public Guid? BtId { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MonsterDefinition_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        public Guid MonsterAiFsmDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MonsterDefinition_fsm_id </summary>
        public virtual MonsterAiFsmDefinitionEntity MonsterAiFsmDefinition { get; set; }
        public Guid MonsterAiBtDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MonsterDefinition_bt_id </summary>
        public virtual MonsterAiBtDefinitionEntity MonsterAiBtDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerMonsterKillLog_monster_definition_id </summary>
        public virtual ICollection<PlayerMonsterKillLogEntity> PlayerMonsterKillLogs { get; set; } = new HashSet<PlayerMonsterKillLogEntity>();
        #endregion
    }
}