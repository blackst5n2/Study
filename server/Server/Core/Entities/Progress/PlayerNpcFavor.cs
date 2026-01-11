using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerNpcFavor
    {
        public Guid PlayerId { get; set; }
        public Guid NpcDefinitionId { get; set; }
        public int FavorPoints { get; set; }
        public int FavorLevel { get; set; }
        public DateTime? LastInteractionAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerNpcFavor_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerNpcFavor_npc_definition_id </summary>
        public virtual NpcDefinition NpcDefinition { get; set; }
        #endregion
    }
}