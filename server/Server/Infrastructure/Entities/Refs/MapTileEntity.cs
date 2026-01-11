using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("MapTile")]
    public class MapTileEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("layer_id")]
        public Guid LayerId { get; set; }
        [Column("x")]
        public int X { get; set; }
        [Column("y")]
        public int Y { get; set; }
        [Column("tile_asset")]
        public string TileAsset { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapLayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapTile_layer_id </summary>
        public virtual MapLayerEntity MapLayer { get; set; }
        #endregion
    }
}