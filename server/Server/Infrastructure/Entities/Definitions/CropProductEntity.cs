using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("CropProduct")]
    public class CropProductEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("crop_definition_id")]
        public Guid CropDefinitionId { get; set; }
        [Column("item_definition_id")]
        public Guid ItemDefinitionId { get; set; }
        [Column("min_yield")]
        public int MinYield { get; set; }
        [Column("max_yield")]
        public int MaxYield { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_CropProduct_crop_definition_id </summary>
        public virtual CropDefinitionEntity CropDefinition { get; set; }
        /// <summary> Relation Label: FK_CropProduct_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}