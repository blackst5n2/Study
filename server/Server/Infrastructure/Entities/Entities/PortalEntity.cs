using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("Portal")]
    public class PortalEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("from_map_id")]
        public Guid FromMapId { get; set; }
        [Column("from_pos_x")]
        public float FromPosX { get; set; }
        [Column("from_pos_y")]
        public float FromPosY { get; set; }
        [Column("from_pos_z")]
        public float FromPosZ { get; set; }
        [Column("from_radius")]
        public float FromRadius { get; set; }
        [Column("to_map_id")]
        public Guid ToMapId { get; set; }
        [Column("to_pos_x")]
        public float ToPosX { get; set; }
        [Column("to_pos_y")]
        public float ToPosY { get; set; }
        [Column("to_pos_z")]
        public float ToPosZ { get; set; }
        [Column("to_direction")]
        public float? ToDirection { get; set; }
        [Column("portal_type")]
        public PortalType PortalType { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("is_bidirectional")]
        public bool IsBidirectional { get; set; }
        [Column("required_level")]
        public int? RequiredLevel { get; set; }
        [Column("required_quest_code")]
        public string RequiredQuestCode { get; set; }
        [Column("required_item_code")]
        public string RequiredItemCode { get; set; }
        [Column("required_item_consumed")]
        public bool RequiredItemConsumed { get; set; }
        [Column("entry_condition")]
        public string EntryCondition { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_Portal_from_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        /// <summary> Relation Label: FK_PortalUseLog_portal_id </summary>
        public virtual ICollection<PortalUseLogEntity> PortalUseLogs { get; set; } = new HashSet<PortalUseLogEntity>();
        /// <summary> Relation Label: FK_DungeonDefinition_entry_portal_id </summary>
        public virtual ICollection<DungeonDefinitionEntity> DungeonDefinitions { get; set; } = new HashSet<DungeonDefinitionEntity>();
        #endregion
    }
}