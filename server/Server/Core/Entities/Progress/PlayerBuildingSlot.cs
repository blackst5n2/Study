
namespace Server.Core.Entities.Progress
{
    public class PlayerBuildingSlot
    {
        public Guid Id { get; set; }
        public Guid PlayerBuildingId { get; set; }
        public string SlotType { get; set; }
        public int SlotIndex { get; set; }
        public Guid? ItemInstanceId { get; set; }
        public Guid? ResearchProjectId { get; set; }
        public Guid? LivestockInstanceId { get; set; }
        public Guid? CropInstanceId { get; set; }
        public string Status { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBuildingSlot_player_building_id </summary>
        public virtual PlayerBuilding PlayerBuilding { get; set; }
        /// <summary> Relation Label: FK_PlayerBuildingSlot_item_instance_id </summary>
        public virtual ItemInstance ItemInstance { get; set; }
        /// <summary> Relation Label: FK_PlayerBuildingSlot_livestock_instance_id </summary>
        public virtual PlayerLivestockInstance PlayerLivestockInstance { get; set; }
        /// <summary> Relation Label: FK_PlayerBuildingSlot_crop_instance_id </summary>
        public virtual PlayerCropInstance PlayerCropInstance { get; set; }
        #endregion
    }
}