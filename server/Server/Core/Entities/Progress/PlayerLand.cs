using Server.Core.Entities.Entities;
using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerLand
    {
        public Guid Id { get; set; }
        public LandOwnershipType OwnerType { get; set; }
        public Guid OwnerId { get; set; }
        public Guid LandPlotId { get; set; }
        public DateTime PurchasedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string CustomName { get; set; }
        public bool IsPrimary { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerLand_land_plot_id </summary>
        public virtual LandPlot LandPlot { get; set; }
        /// <summary> Relation Label: FK_PlayerBuilding_land_plot_id </summary>
        public virtual ICollection<PlayerBuilding> PlayerBuildings { get; set; } = new HashSet<PlayerBuilding>();
        #endregion
    }
}