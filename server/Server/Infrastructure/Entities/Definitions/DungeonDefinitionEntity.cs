using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("DungeonDefinition")]
    public class DungeonDefinitionEntity
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
        [Column("entry_portal_id")]
        public Guid? EntryPortalId { get; set; }
        [Column("min_players")]
        public int MinPlayers { get; set; }
        [Column("max_players")]
        public int MaxPlayers { get; set; }
        [Column("level_requirement")]
        public int LevelRequirement { get; set; }
        [Column("reward_preview")]
        public string RewardPreview { get; set; }
        [Column("is_random")]
        public bool IsRandom { get; set; }
        [Column("has_boss")]
        public bool HasBoss { get; set; }
        [Column("time_limit_seconds")]
        public int? TimeLimitSeconds { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PortalId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_DungeonDefinition_entry_portal_id </summary>
        public virtual PortalEntity Portal { get; set; }
        /// <summary> Relation Label: FK_DungeonZoneLink_dungeon_id </summary>
        public virtual ICollection<DungeonZoneLinkEntity> DungeonZoneLinks { get; set; } = new HashSet<DungeonZoneLinkEntity>();
        /// <summary> Relation Label: FK_DungeonRun_dungeon_id </summary>
        public virtual ICollection<DungeonRunEntity> DungeonRuns { get; set; } = new HashSet<DungeonRunEntity>();
        #endregion
    }
}