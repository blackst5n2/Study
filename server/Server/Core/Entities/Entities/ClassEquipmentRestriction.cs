using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class ClassEquipmentRestriction
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public ItemType ItemType { get; set; }
        public ItemSubType ItemSubType { get; set; }
        public Guid? ItemTagId { get; set; }
        public RestrictionType RestrictionType { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ClassEquipmentRestriction_class_id </summary>
        public virtual ClassDefinition ClassDefinition { get; set; }
        /// <summary> Relation Label: FK_ClassEquipmentRestriction_item_tag_id </summary>
        public virtual Tag Tag { get; set; }
        #endregion
    }
}