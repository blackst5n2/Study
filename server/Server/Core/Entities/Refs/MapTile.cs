
namespace Server.Core.Entities.Refs
{
    public class MapTile
    {
        public Guid Id { get; set; }
        public Guid LayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string TileAsset { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapTile_layer_id </summary>
        public virtual MapLayer MapLayer { get; set; }
        #endregion
    }
}