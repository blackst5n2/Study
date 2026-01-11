using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("DungeonZoneLink")]
    public class DungeonZoneLinkEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("dungeon_id")]
        public Guid DungeonId { get; set; }
        [Column("map_zone_id")]
        public Guid MapZoneId { get; set; }
        [Column("sequence")]
        public int Sequence { get; set; }
        [Column("zone_type")]
        public DungeonZoneType ZoneType { get; set; }
        [Column("is_start_zone")]
        public bool IsStartZone { get; set; }
        [Column("is_end_zone")]
        public bool IsEndZone { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid DungeonDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_DungeonZoneLink_dungeon_id </summary>
        public virtual DungeonDefinitionEntity DungeonDefinition { get; set; }
        /// <summary> Relation Label: FK_DungeonZoneLink_map_zone_id </summary>
        public virtual MapZoneEntity MapZone { get; set; }
        #endregion
    }
}