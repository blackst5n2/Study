using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class NpcDefinition
    {
        public Guid Id { get; set; }
        public Guid EntityDefinitionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public NpcType NpcType { get; set; }
        public string Description { get; set; }
        public string SpriteAsset { get; set; }
        public string ModelAsset { get; set; }
        public string BehaviorScript { get; set; }
        public string DialogueStartCode { get; set; }
        public Guid? ShopId { get; set; }
        public bool IsQuestGiver { get; set; }
        public string Faction { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_NpcDefinition_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_NpcDefinition_shop_id </summary>
        public virtual ShopDefinition ShopDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerNpcFavor_npc_definition_id </summary>
        public virtual ICollection<PlayerNpcFavor> PlayerNpcFavors { get; set; } = new HashSet<PlayerNpcFavor>();
        /// <summary> Relation Label: FK_QuestDefinition_start_npc_code </summary>
        public virtual ICollection<QuestDefinition> QuestDefinitions { get; set; } = new HashSet<QuestDefinition>();
        #endregion
    }
}