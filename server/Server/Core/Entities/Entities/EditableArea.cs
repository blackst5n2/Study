using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class EditableArea
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public MapZoneAreaType AreaType { get; set; }
        public string AreaData { get; set; }
        public bool IsBuildable { get; set; }
        public bool IsFarmable { get; set; }
        public string UnlockCondition { get; set; }
        public LandOwnershipType OwnerType { get; set; }
        public Guid? OwnerId { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_EditableArea_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        #endregion
    }
}