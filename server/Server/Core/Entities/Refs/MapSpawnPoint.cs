using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Refs
{
    public class MapSpawnPoint
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public Guid? EntityDefinitionId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float SpawnRadius { get; set; }
        public float? InitialDirection { get; set; }
        public int MaxConcurrent { get; set; }
        public int? RespawnTimeSeconds { get; set; }
        public string SpawnCondition { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapSpawnPoint_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        /// <summary> Relation Label: FK_MapSpawnPoint_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        #endregion
    }
}