
namespace Server.Core.Entities.Definitions
{
    public class BuildingUpgradeDefinition
    {
        public Guid Id { get; set; }
        public Guid BuildingDefinitionId { get; set; }
        public int Level { get; set; }
        public string UpgradeEffectDescription { get; set; }
        public int UpgradeTimeSeconds { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_BuildingUpgradeDefinition_building_definition_id </summary>
        public virtual BuildingDefinition BuildingDefinition { get; set; }
        /// <summary> Relation Label: FK_BuildingUpgradeEffect_upgrade_definition_id </summary>
        public virtual ICollection<BuildingUpgradeEffect> BuildingUpgradeEffects { get; set; } = new HashSet<BuildingUpgradeEffect>();
        #endregion
    }
}