using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Refs
{
    public class DungeonZoneLink
    {
        public Guid Id { get; set; }
        public Guid DungeonId { get; set; }
        public Guid MapZoneId { get; set; }
        public int Sequence { get; set; }
        public DungeonZoneType ZoneType { get; set; }
        public bool IsStartZone { get; set; }
        public bool IsEndZone { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DungeonZoneLink_dungeon_id </summary>
        public virtual DungeonDefinition DungeonDefinition { get; set; }
        /// <summary> Relation Label: FK_DungeonZoneLink_map_zone_id </summary>
        public virtual MapZone MapZone { get; set; }
        #endregion
    }
}