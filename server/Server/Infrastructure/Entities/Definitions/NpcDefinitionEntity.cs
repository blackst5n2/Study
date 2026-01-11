using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("NpcDefinition")]
    public class NpcDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("entity_definition_id")]
        public Guid EntityDefinitionId { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("npc_type")]
        public NpcType NpcType { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("sprite_asset")]
        public string SpriteAsset { get; set; }
        [Column("model_asset")]
        public string ModelAsset { get; set; }
        [Column("behavior_script")]
        public string BehaviorScript { get; set; }
        [Column("dialogue_start_code")]
        public string DialogueStartCode { get; set; }
        [Column("shop_id")]
        public Guid? ShopId { get; set; }
        [Column("is_quest_giver")]
        public bool IsQuestGiver { get; set; }
        [Column("faction")]
        public string Faction { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_NpcDefinition_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        public Guid ShopDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_NpcDefinition_shop_id </summary>
        public virtual ShopDefinitionEntity ShopDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerNpcFavor_npc_definition_id </summary>
        public virtual ICollection<PlayerNpcFavorEntity> PlayerNpcFavors { get; set; } = new HashSet<PlayerNpcFavorEntity>();
        /// <summary> Relation Label: FK_QuestDefinition_start_npc_code </summary>
        public virtual ICollection<QuestDefinitionEntity> QuestDefinitions { get; set; } = new HashSet<QuestDefinitionEntity>();
        #endregion
    }
}