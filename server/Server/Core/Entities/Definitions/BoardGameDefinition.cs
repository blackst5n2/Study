using Server.Core.Entities.Entities;

namespace Server.Core.Entities.Definitions
{
    public class BoardGameDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxPlayer { get; set; }
        public int MinPlayer { get; set; }
        public string Rules { get; set; }
        public DateTime CreatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_BoardGameRoom_board_game_id </summary>
        public virtual ICollection<BoardGameRoom> BoardGameRooms { get; set; } = new HashSet<BoardGameRoom>();
        #endregion
    }
}