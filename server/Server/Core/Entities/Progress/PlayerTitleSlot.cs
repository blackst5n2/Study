
namespace Server.Core.Entities.Progress
{
    public class PlayerTitleSlot
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public int SlotIndex { get; set; }
        public Guid? PlayerTitleId { get; set; }
        public DateTime? EquippedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerTitleSlot_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerTitleSlot_player_title_id </summary>
        public virtual PlayerTitle PlayerTitle { get; set; }
        #endregion
    }
}