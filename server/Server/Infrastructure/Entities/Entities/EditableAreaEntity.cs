using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("EditableArea")]
    public class EditableAreaEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("area_type")]
        public MapZoneAreaType AreaType { get; set; }
        [Column("area_data")]
        public string AreaData { get; set; }
        [Column("is_buildable")]
        public bool IsBuildable { get; set; }
        [Column("is_farmable")]
        public bool IsFarmable { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("owner_type")]
        public LandOwnershipType OwnerType { get; set; }
        [Column("owner_id")]
        public Guid? OwnerId { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_EditableArea_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        #endregion
    }
}