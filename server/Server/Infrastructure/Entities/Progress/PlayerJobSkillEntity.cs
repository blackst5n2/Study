using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerJobSkill")]
    public class PlayerJobSkillEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("skill_id")]
        public Guid SkillId { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("acquired_at")]
        public DateTime AcquiredAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerJobSkill_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid JobSkillDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerJobSkill_skill_id </summary>
        public virtual JobSkillDefinitionEntity JobSkillDefinition { get; set; }
        #endregion
    }
}