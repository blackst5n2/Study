using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("MapSpawnPoint")]
    public class MapSpawnPointEntity
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
        [Column("spawn_radius")]
        public float SpawnRadius { get; set; }
        [Column("initial_direction")]
        public float? InitialDirection { get; set; }
        [Column("max_concurrent")]
        public int MaxConcurrent { get; set; }
        [Column("respawn_time_seconds")]
        public int? RespawnTimeSeconds { get; set; }
        [Column("spawn_condition")]
        public string SpawnCondition { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapSpawnPoint_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        /// <summary> Relation Label: FK_MapSpawnPoint_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        #endregion
    }
}