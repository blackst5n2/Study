using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerSkill")]
    public class PlayerSkillEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("skill_definition_id")]
        public Guid SkillDefinitionId { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("is_learned")]
        public bool IsLearned { get; set; }
        [Column("acquired_at")]
        public DateTime? AcquiredAt { get; set; }
        [Column("last_used_at")]
        public DateTime? LastUsedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerSkill_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerSkill_skill_definition_id </summary>
        public virtual SkillDefinitionEntity SkillDefinition { get; set; }
        #endregion
    }
}