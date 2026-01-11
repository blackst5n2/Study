using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerJobSkillPoint")]
    public class PlayerJobSkillPointEntity
    {
        // 복합키: [player_id, job_id] -> OnModelCreating에서 HasKey 설정 필요
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("job_id")]
        public Guid JobId { get; set; }
        [Column("total_points")]
        public int TotalPoints { get; set; }
        [Column("used_points")]
        public int UsedPoints { get; set; }
        [Column("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerJobSkillPoint_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid JobDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerJobSkillPoint_job_id </summary>
        public virtual JobDefinitionEntity JobDefinition { get; set; }
        #endregion
    }
}