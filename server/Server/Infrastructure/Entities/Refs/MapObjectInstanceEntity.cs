using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("MapObjectInstance")]
    public class MapObjectInstanceEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("entity_definition_id")]
        public Guid? EntityDefinitionId { get; set; }
        [Column("pos_x")]
        public float PosX { get; set; }
        [Column("pos_y")]
        public float PosY { get; set; }
        [Column("pos_z")]
        public float PosZ { get; set; }
        [Column("rotation_x")]
        public float RotationX { get; set; }
        [Column("rotation_y")]
        public float RotationY { get; set; }
        [Column("rotation_z")]
        public float RotationZ { get; set; }
        [Column("scale_x")]
        public float ScaleX { get; set; }
        [Column("scale_y")]
        public float ScaleY { get; set; }
        [Column("scale_z")]
        public float ScaleZ { get; set; }
        [Column("spawn_condition")]
        public string SpawnCondition { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapObjectInstance_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        /// <summary> Relation Label: FK_MapObjectInstance_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        #endregion
    }
}