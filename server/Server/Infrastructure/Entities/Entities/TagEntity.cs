using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("Tag")]
    public class TagEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ClassEquipmentRestriction_item_tag_id </summary>
        public virtual ICollection<ClassEquipmentRestrictionEntity> ClassEquipmentRestrictions { get; set; } = new HashSet<ClassEquipmentRestrictionEntity>();
        /// <summary> Relation Label: N:M via ItemTag </summary>
        public virtual ICollection<ItemDefinitionEntity> ItemDefinitions { get; set; } = new HashSet<ItemDefinitionEntity>();
        #endregion
    }
}