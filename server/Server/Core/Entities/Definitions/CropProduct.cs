
namespace Server.Core.Entities.Definitions
{
    public class CropProduct
    {
        public Guid Id { get; set; }
        public Guid CropDefinitionId { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public int MinYield { get; set; }
        public int MaxYield { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_CropProduct_crop_definition_id </summary>
        public virtual CropDefinition CropDefinition { get; set; }
        /// <summary> Relation Label: FK_CropProduct_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}