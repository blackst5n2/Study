using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class LandPlot
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public LandPlotType PlotType { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsPublic { get; set; }
        public int? Price { get; set; }
        public string UnlockCondition { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_LandPlot_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerLand_land_plot_id </summary>
        public virtual ICollection<PlayerLand> PlayerLands { get; set; } = new HashSet<PlayerLand>();
        #endregion
    }
}