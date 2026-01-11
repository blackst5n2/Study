using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Refs
{
    public class MapObjectInstance
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public Guid? EntityDefinitionId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float ScaleZ { get; set; }
        public string SpawnCondition { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapObjectInstance_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        /// <summary> Relation Label: FK_MapObjectInstance_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        #endregion
    }
}