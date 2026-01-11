using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("TitleCategory")]
    public class TitleCategoryEntity
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
        [Column("display_order")]
        public int DisplayOrder { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_TitleDefinition_category_id </summary>
        public virtual ICollection<TitleDefinitionEntity> TitleDefinitions { get; set; } = new HashSet<TitleDefinitionEntity>();
        #endregion
    }
}