using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("MapDynamicObjectState")]
    public class MapDynamicObjectStateEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("object_instance_id")]
        public Guid ObjectInstanceId { get; set; }
        [Column("state")]
        public string State { get; set; }
        [Column("health")]
        public int? Health { get; set; }
        [Column("growth_stage")]
        public int? GrowthStage { get; set; }
        [Column("growth_timer")]
        public float? GrowthTimer { get; set; }
        [Column("last_changed_at")]
        public DateTime LastChangedAt { get; set; }
        [Column("owner_id")]
        public Guid? OwnerId { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapDynamicObjectState_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        #endregion
    }
}