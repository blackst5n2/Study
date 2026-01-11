using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerBuildingSlot")]
    public class PlayerBuildingSlotEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_building_id")]
        public Guid PlayerBuildingId { get; set; }
        [Column("slot_type")]
        public string SlotType { get; set; }
        [Column("slot_index")]
        public int SlotIndex { get; set; }
        [Column("item_instance_id")]
        public Guid? ItemInstanceId { get; set; }
        [Column("research_project_id")]
        public Guid? ResearchProjectId { get; set; }
        [Column("livestock_instance_id")]
        public Guid? LivestockInstanceId { get; set; }
        [Column("crop_instance_id")]
        public Guid? CropInstanceId { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("started_at")]
        public DateTime? StartedAt { get; set; }
        [Column("finished_at")]
        public DateTime? FinishedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBuildingSlot_player_building_id </summary>
        public virtual PlayerBuildingEntity PlayerBuilding { get; set; }
        /// <summary> Relation Label: FK_PlayerBuildingSlot_item_instance_id </summary>
        public virtual ItemInstanceEntity ItemInstance { get; set; }
        public Guid PlayerLivestockInstanceId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerBuildingSlot_livestock_instance_id </summary>
        public virtual PlayerLivestockInstanceEntity PlayerLivestockInstance { get; set; }
        public Guid PlayerCropInstanceId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerBuildingSlot_crop_instance_id </summary>
        public virtual PlayerCropInstanceEntity PlayerCropInstance { get; set; }
        #endregion
    }
}