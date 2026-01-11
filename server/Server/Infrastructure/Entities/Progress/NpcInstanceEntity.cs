using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("NpcInstance")]
    public class NpcInstanceEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("npc_definition_id")]
        public Guid NpcDefinitionId { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("zone_id")]
        public Guid? ZoneId { get; set; }
        [Column("pos_x")]
        public float PosX { get; set; }
        [Column("pos_y")]
        public float PosY { get; set; }
        [Column("pos_z")]
        public float PosZ { get; set; }
        [Column("rotation_y")]
        public float RotationY { get; set; }
        [Column("spawn_condition")]
        public string SpawnCondition { get; set; }
        [Column("despawn_condition")]
        public string DespawnCondition { get; set; }
        [Column("current_behavior_state")]
        public string CurrentBehaviorState { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_NpcInstance_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        public Guid MapZoneId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_NpcInstance_zone_id </summary>
        public virtual MapZoneEntity MapZone { get; set; }
        #endregion
    }
}