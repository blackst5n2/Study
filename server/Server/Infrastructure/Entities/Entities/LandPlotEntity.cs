using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("LandPlot")]
    public class LandPlotEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("plot_type")]
        public LandPlotType PlotType { get; set; }
        [Column("coord_x")]
        public int CoordX { get; set; }
        [Column("coord_y")]
        public int CoordY { get; set; }
        [Column("width")]
        public int Width { get; set; }
        [Column("height")]
        public int Height { get; set; }
        [Column("is_public")]
        public bool IsPublic { get; set; }
        [Column("price")]
        public int? Price { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_LandPlot_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerLand_land_plot_id </summary>
        public virtual ICollection<PlayerLandEntity> PlayerLands { get; set; } = new HashSet<PlayerLandEntity>();
        #endregion
    }
}