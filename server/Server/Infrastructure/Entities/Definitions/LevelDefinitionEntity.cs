using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("LevelDefinition")]
    public class LevelDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("exp_required")]
        public long ExpRequired { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerLevelLog_level_code </summary>
        public virtual ICollection<PlayerLevelLogEntity> PlayerLevelLogs { get; set; } = new HashSet<PlayerLevelLogEntity>();
        #endregion
    }
}