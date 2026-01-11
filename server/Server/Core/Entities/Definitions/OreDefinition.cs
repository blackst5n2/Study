using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class OreDefinition
    {
        public Guid Id { get; set; }
        public Guid EntityDefinitionId { get; set; }
        public ItemGrade Grade { get; set; }
        public ItemSubType RequiredToolSubtype { get; set; }
        public int RequiredToolLevel { get; set; }
        public int RequiredSkillLevel { get; set; }
        public int MiningDifficulty { get; set; }
        public int? RespawnTimeSeconds { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_OreDefinition_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerMiningLog_ore_definition_id </summary>
        public virtual ICollection<PlayerMiningLog> PlayerMiningLogs { get; set; } = new HashSet<PlayerMiningLog>();
        #endregion
    }
}