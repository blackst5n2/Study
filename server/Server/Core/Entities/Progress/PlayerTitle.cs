using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerTitle
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TitleId { get; set; }
        public DateTime AcquiredAt { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerTitle_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerTitle_title_id </summary>
        public virtual TitleDefinition TitleDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerTitleSlot_player_title_id </summary>
        public virtual ICollection<PlayerTitleSlot> PlayerTitleSlots { get; set; } = new HashSet<PlayerTitleSlot>();
        #endregion
    }
}