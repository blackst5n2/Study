using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("BuffDefinition")]
    public class BuffDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("category")]
        public BuffCategory Category { get; set; }
        [Column("effect_type")]
        public BuffType EffectType { get; set; }
        [Column("value_formula")]
        public string ValueFormula { get; set; }
        [Column("duration_seconds")]
        public float? DurationSeconds { get; set; }
        [Column("max_stack")]
        public int MaxStack { get; set; }
        [Column("is_dispellable")]
        public bool IsDispellable { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("particle_effect")]
        public string ParticleEffect { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBuff_buff_definition_id </summary>
        public virtual ICollection<PlayerBuffEntity> PlayerBuffs { get; set; } = new HashSet<PlayerBuffEntity>();
        /// <summary> Relation Label: FK_PlayerBuffLog_buff_definition_id </summary>
        public virtual ICollection<PlayerBuffLogEntity> PlayerBuffLogs { get; set; } = new HashSet<PlayerBuffLogEntity>();
        /// <summary> Relation Label: FK_SkillGrantedBuff_buff_definition_id </summary>
        public virtual ICollection<SkillGrantedBuffEntity> SkillGrantedBuffs { get; set; } = new HashSet<SkillGrantedBuffEntity>();
        #endregion
    }
}