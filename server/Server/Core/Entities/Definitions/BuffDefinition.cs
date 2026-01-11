using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class BuffDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BuffCategory Category { get; set; }
        public BuffType EffectType { get; set; }
        public string ValueFormula { get; set; }
        public float? DurationSeconds { get; set; }
        public int MaxStack { get; set; }
        public bool IsDispellable { get; set; }
        public string Icon { get; set; }
        public string ParticleEffect { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBuff_buff_definition_id </summary>
        public virtual ICollection<PlayerBuff> PlayerBuffs { get; set; } = new HashSet<PlayerBuff>();
        /// <summary> Relation Label: FK_PlayerBuffLog_buff_definition_id </summary>
        public virtual ICollection<PlayerBuffLog> PlayerBuffLogs { get; set; } = new HashSet<PlayerBuffLog>();
        /// <summary> Relation Label: FK_SkillGrantedBuff_buff_definition_id </summary>
        public virtual ICollection<SkillGrantedBuff> SkillGrantedBuffs { get; set; } = new HashSet<SkillGrantedBuff>();
        #endregion
    }
}