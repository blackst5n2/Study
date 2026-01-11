using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class PartyMember
    {
        public Guid Id { get; set; }
        public Guid PartyId { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public PartyRole Role { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PartyMember_party_id </summary>
        public virtual PartyDefinition PartyDefinition { get; set; }
        /// <summary> Relation Label: FK_PartyMember_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}