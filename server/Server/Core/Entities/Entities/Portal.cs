using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class Portal
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid FromMapId { get; set; }
        public float FromPosX { get; set; }
        public float FromPosY { get; set; }
        public float FromPosZ { get; set; }
        public float FromRadius { get; set; }
        public Guid ToMapId { get; set; }
        public float ToPosX { get; set; }
        public float ToPosY { get; set; }
        public float ToPosZ { get; set; }
        public float? ToDirection { get; set; }
        public PortalType PortalType { get; set; }
        public bool IsActive { get; set; }
        public bool IsBidirectional { get; set; }
        public int? RequiredLevel { get; set; }
        public string RequiredQuestCode { get; set; }
        public string RequiredItemCode { get; set; }
        public bool RequiredItemConsumed { get; set; }
        public string EntryCondition { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Portal_from_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        /// <summary> Relation Label: FK_PortalUseLog_portal_id </summary>
        public virtual ICollection<PortalUseLog> PortalUseLogs { get; set; } = new HashSet<PortalUseLog>();
        /// <summary> Relation Label: FK_DungeonDefinition_entry_portal_id </summary>
        public virtual ICollection<DungeonDefinition> DungeonDefinitions { get; set; } = new HashSet<DungeonDefinition>();
        #endregion
    }
}