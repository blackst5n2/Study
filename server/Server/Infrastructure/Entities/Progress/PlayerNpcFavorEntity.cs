using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerNpcFavor")]
    public class PlayerNpcFavorEntity
    {
        // 복합키: [player_id, npc_definition_id] -> OnModelCreating에서 HasKey 설정 필요
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("npc_definition_id")]
        public Guid NpcDefinitionId { get; set; }
        [Column("favor_points")]
        public int FavorPoints { get; set; }
        [Column("favor_level")]
        public int FavorLevel { get; set; }
        [Column("last_interaction_at")]
        public DateTime? LastInteractionAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerNpcFavor_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerNpcFavor_npc_definition_id </summary>
        public virtual NpcDefinitionEntity NpcDefinition { get; set; }
        #endregion
    }
}