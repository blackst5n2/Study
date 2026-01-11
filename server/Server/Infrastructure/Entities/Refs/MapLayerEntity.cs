using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("MapLayer")]
    public class MapLayerEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("layer_order")]
        public int LayerOrder { get; set; }
        [Column("type")]
        public MapLayerType Type { get; set; }
        [Column("is_visible")]
        public bool IsVisible { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapLayer_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        /// <summary> Relation Label: FK_MapTile_layer_id </summary>
        public virtual ICollection<MapTileEntity> MapTiles { get; set; } = new HashSet<MapTileEntity>();
        #endregion
    }
}