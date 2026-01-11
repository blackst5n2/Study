using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ClassEquipmentRestriction_item_tag_id </summary>
        public virtual ICollection<ClassEquipmentRestriction> ClassEquipmentRestrictions { get; set; } = new HashSet<ClassEquipmentRestriction>();
        /// <summary> Relation Label: N:M via ItemTag </summary>
        public virtual ICollection<ItemDefinition> ItemDefinitions { get; set; } = new HashSet<ItemDefinition>();
        #endregion
    }
}