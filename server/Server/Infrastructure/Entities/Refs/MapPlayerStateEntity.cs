using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("MapPlayerState")]
    public class MapPlayerStateEntity
    {
        [Column("player_id")]
        [Key]
        public Guid PlayerId { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("pos_x")]
        public float PosX { get; set; }
        [Column("pos_y")]
        public float PosY { get; set; }
        [Column("pos_z")]
        public float PosZ { get; set; }
        [Column("rotation_y")]
        public float? RotationY { get; set; }
        [Column("state")]
        public string State { get; set; }
        [Column("last_sync_at")]
        public DateTime LastSyncAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapPlayerState_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapPlayerState_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        #endregion
    }
}