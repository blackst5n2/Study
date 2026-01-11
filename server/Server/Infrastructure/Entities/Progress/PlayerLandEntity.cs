using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerLand")]
    public class PlayerLandEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("owner_type")]
        public LandOwnershipType OwnerType { get; set; }
        [Column("owner_id")]
        public Guid OwnerId { get; set; }
        [Column("land_plot_id")]
        public Guid LandPlotId { get; set; }
        [Column("purchased_at")]
        public DateTime PurchasedAt { get; set; }
        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
        [Column("custom_name")]
        public string CustomName { get; set; }
        [Column("is_primary")]
        public bool IsPrimary { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerLand_land_plot_id </summary>
        public virtual LandPlotEntity LandPlot { get; set; }
        /// <summary> Relation Label: FK_PlayerBuilding_land_plot_id </summary>
        public virtual ICollection<PlayerBuildingEntity> PlayerBuildings { get; set; } = new HashSet<PlayerBuildingEntity>();
        #endregion
    }
}