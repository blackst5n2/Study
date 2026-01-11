using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Refs
{
    public class MapDynamicObjectState
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public Guid ObjectInstanceId { get; set; }
        public string State { get; set; }
        public int? Health { get; set; }
        public int? GrowthStage { get; set; }
        public float? GrowthTimer { get; set; }
        public DateTime LastChangedAt { get; set; }
        public Guid? OwnerId { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapDynamicObjectState_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        #endregion
    }
}