using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("ClassEquipmentRestriction")]
    public class ClassEquipmentRestrictionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("class_id")]
        public Guid ClassId { get; set; }
        [Column("item_type")]
        public ItemType ItemType { get; set; }
        [Column("item_sub_type")]
        public ItemSubType ItemSubType { get; set; }
        [Column("item_tag_id")]
        public Guid? ItemTagId { get; set; }
        [Column("restriction_type")]
        public RestrictionType RestrictionType { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid ClassDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ClassEquipmentRestriction_class_id </summary>
        public virtual ClassDefinitionEntity ClassDefinition { get; set; }
        public Guid TagId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ClassEquipmentRestriction_item_tag_id </summary>
        public virtual TagEntity Tag { get; set; }
        #endregion
    }
}