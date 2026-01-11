using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Refs
{
    public class MapLayer
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public string Name { get; set; }
        public int LayerOrder { get; set; }
        public MapLayerType Type { get; set; }
        public bool IsVisible { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapLayer_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        /// <summary> Relation Label: FK_MapTile_layer_id </summary>
        public virtual ICollection<MapTile> MapTiles { get; set; } = new HashSet<MapTile>();
        #endregion
    }
}