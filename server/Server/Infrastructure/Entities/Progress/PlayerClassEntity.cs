using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerClass")]
    public class PlayerClassEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("class_id")]
        public Guid ClassId { get; set; }
        [Column("is_main")]
        public bool IsMain { get; set; }
        [Column("is_unlocked")]
        public bool IsUnlocked { get; set; }
        [Column("acquired_at")]
        public DateTime? AcquiredAt { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("exp")]
        public int Exp { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerClass_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid ClassDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerClass_class_id </summary>
        public virtual ClassDefinitionEntity ClassDefinition { get; set; }
        #endregion
    }
}