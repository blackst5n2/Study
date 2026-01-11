using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("PartyMember")]
    public class PartyMemberEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("party_id")]
        public Guid PartyId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("joined_at")]
        public DateTime JoinedAt { get; set; }
        [Column("left_at")]
        public DateTime? LeftAt { get; set; }
        [Column("role")]
        public PartyRole Role { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PartyDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PartyMember_party_id </summary>
        public virtual PartyDefinitionEntity PartyDefinition { get; set; }
        /// <summary> Relation Label: FK_PartyMember_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}