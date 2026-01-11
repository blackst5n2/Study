using Server.Infrastructure.Entities.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("BoardGameDefinition")]
    public class BoardGameDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("max_player")]
        public int MaxPlayer { get; set; }
        [Column("min_player")]
        public int MinPlayer { get; set; }
        [Column("rules")]
        public string Rules { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_BoardGameRoom_board_game_id </summary>
        public virtual ICollection<BoardGameRoomEntity> BoardGameRooms { get; set; } = new HashSet<BoardGameRoomEntity>();
        #endregion
    }
}