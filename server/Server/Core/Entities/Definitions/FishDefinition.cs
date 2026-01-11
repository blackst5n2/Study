using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class FishDefinition
    {
        public Guid Id { get; set; }
        public Guid EntityDefinitionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ItemGrade Grade { get; set; }
        public int RequiredFishingLevel { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public float? MinLengthCm { get; set; }
        public float? MaxLengthCm { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_FishDefinition_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_FishHabitat_fish_definition_id </summary>
        public virtual ICollection<FishHabitat> FishHabitats { get; set; } = new HashSet<FishHabitat>();
        /// <summary> Relation Label: FK_PlayerFishingLog_fish_definition_id </summary>
        public virtual ICollection<PlayerFishingLog> PlayerFishingLogs { get; set; } = new HashSet<PlayerFishingLog>();
        /// <summary> Relation Label: FK_FishDefinition_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}