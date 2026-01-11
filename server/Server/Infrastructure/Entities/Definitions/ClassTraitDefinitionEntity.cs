using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("ClassTraitDefinition")]
    public class ClassTraitDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("class_id")]
        public Guid ClassId { get; set; }
        [Column("trait_name")]
        public string TraitName { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("effect_data")]
        public string EffectData { get; set; }

        #region Navigation Properties
        public Guid ClassDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ClassTraitDefinition_class_id </summary>
        public virtual ClassDefinitionEntity ClassDefinition { get; set; }
        #endregion
    }
}