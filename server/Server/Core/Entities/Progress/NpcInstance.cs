using Server.Core.Entities.Definitions;
using Server.Core.Entities.Refs;

namespace Server.Core.Entities.Progress
{
    public class NpcInstance
    {
        public Guid Id { get; set; }
        public Guid NpcDefinitionId { get; set; }
        public Guid MapId { get; set; }
        public Guid? ZoneId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotationY { get; set; }
        public string SpawnCondition { get; set; }
        public string DespawnCondition { get; set; }
        public string CurrentBehaviorState { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_NpcInstance_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        /// <summary> Relation Label: FK_NpcInstance_zone_id </summary>
        public virtual MapZone MapZone { get; set; }
        #endregion
    }
}