
namespace Server.Core.Entities.Definitions
{
    public class ClassTraitDefinition
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string TraitName { get; set; }
        public string Description { get; set; }
        public string EffectData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ClassTraitDefinition_class_id </summary>
        public virtual ClassDefinition ClassDefinition { get; set; }
        #endregion
    }
}