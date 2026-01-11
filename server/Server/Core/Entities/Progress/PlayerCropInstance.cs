using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerCropInstance
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid CropDefinitionId { get; set; }
        public Guid? FarmPlotId { get; set; }
        public Guid? LandPlotId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public DateTime PlantTime { get; set; }
        public int GrowthStage { get; set; }
        public float CurrentGrowthTimeMinutes { get; set; }
        public bool IsWatered { get; set; }
        public DateTime? LastWateredAt { get; set; }
        public string DiseaseState { get; set; }
        public DateTime? ExpectedHarvestTime { get; set; }
        public DateTime? ActualHarvestTime { get; set; }
        public string AppliedFertilizer { get; set; }
        public CropGrade CurrentGrade { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerCropInstance_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerCropInstance_crop_definition_id </summary>
        public virtual CropDefinition CropDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerCropHarvestLog_player_crop_instance_id </summary>
        public virtual ICollection<PlayerCropHarvestLog> PlayerCropHarvestLogs { get; set; } = new HashSet<PlayerCropHarvestLog>();
        /// <summary> Relation Label: FK_PlayerBuildingSlot_crop_instance_id </summary>
        public virtual ICollection<PlayerBuildingSlot> PlayerBuildingSlots { get; set; } = new HashSet<PlayerBuildingSlot>();
        #endregion
    }
}