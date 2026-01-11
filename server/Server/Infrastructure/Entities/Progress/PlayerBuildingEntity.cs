using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerBuilding")]
    public class PlayerBuildingEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("land_plot_id")]
        public Guid LandPlotId { get; set; }
        [Column("building_definition_id")]
        public Guid BuildingDefinitionId { get; set; }
        [Column("inventory_item_id")]
        public Guid? InventoryItemId { get; set; }
        [Column("pos_x")]
        public float PosX { get; set; }
        [Column("pos_y")]
        public float PosY { get; set; }
        [Column("pos_z")]
        public float PosZ { get; set; }
        [Column("rotation_y")]
        public float RotationY { get; set; }
        [Column("placed_at")]
        public DateTime PlacedAt { get; set; }
        [Column("removed_at")]
        public DateTime? RemovedAt { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("current_upgrade_finish_time")]
        public DateTime? CurrentUpgradeFinishTime { get; set; }
        [Column("durability")]
        public int? Durability { get; set; }
        [Column("status")]
        public BuildingStatus Status { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBuilding_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid PlayerLandId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerBuilding_land_plot_id </summary>
        public virtual PlayerLandEntity PlayerLand { get; set; }
        /// <summary> Relation Label: FK_PlayerBuilding_building_definition_id </summary>
        public virtual BuildingDefinitionEntity BuildingDefinition { get; set; }
        /// <summary> Relation Label: FK_BuildingActionLog_player_building_id </summary>
        public virtual ICollection<BuildingActionLogEntity> BuildingActionLogs { get; set; } = new HashSet<BuildingActionLogEntity>();
        /// <summary> Relation Label: FK_PlayerBuildingSlot_player_building_id </summary>
        public virtual ICollection<PlayerBuildingSlotEntity> PlayerBuildingSlots { get; set; } = new HashSet<PlayerBuildingSlotEntity>();
        #endregion
    }
}