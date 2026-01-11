using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerBuilding
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid LandPlotId { get; set; }
        public Guid BuildingDefinitionId { get; set; }
        public Guid? InventoryItemId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotationY { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public int Level { get; set; }
        public DateTime? CurrentUpgradeFinishTime { get; set; }
        public int? Durability { get; set; }
        public BuildingStatus Status { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBuilding_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerBuilding_land_plot_id </summary>
        public virtual PlayerLand PlayerLand { get; set; }
        /// <summary> Relation Label: FK_PlayerBuilding_building_definition_id </summary>
        public virtual BuildingDefinition BuildingDefinition { get; set; }
        /// <summary> Relation Label: FK_BuildingActionLog_player_building_id </summary>
        public virtual ICollection<BuildingActionLog> BuildingActionLogs { get; set; } = new HashSet<BuildingActionLog>();
        /// <summary> Relation Label: FK_PlayerBuildingSlot_player_building_id </summary>
        public virtual ICollection<PlayerBuildingSlot> PlayerBuildingSlots { get; set; } = new HashSet<PlayerBuildingSlot>();
        #endregion
    }
}