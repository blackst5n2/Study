using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Refs
{
    public class MapPlayerState
    {
        public Guid PlayerId { get; set; }
        public Guid MapId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float? RotationY { get; set; }
        public string State { get; set; }
        public DateTime LastSyncAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapPlayerState_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_MapPlayerState_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        #endregion
    }
}