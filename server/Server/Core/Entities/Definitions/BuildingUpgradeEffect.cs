using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class BuildingUpgradeEffect
    {
        public Guid Id { get; set; }
        public Guid UpgradeDefinitionId { get; set; }
        public BuildingEffectType EffectType { get; set; }
        public string EffectKey { get; set; }
        public string Value { get; set; }
        public string ExtraData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_BuildingUpgradeEffect_upgrade_definition_id </summary>
        public virtual BuildingUpgradeDefinition BuildingUpgradeDefinition { get; set; }
        #endregion
    }
}