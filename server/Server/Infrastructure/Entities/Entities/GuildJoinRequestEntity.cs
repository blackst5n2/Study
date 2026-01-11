using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("GuildJoinRequest")]
    public class GuildJoinRequestEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("guild_id")]
        public Guid GuildId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("status")]
        public GuildJoinRequestStatus Status { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("requested_at")]
        public DateTime RequestedAt { get; set; }
        [Column("processed_at")]
        public DateTime? ProcessedAt { get; set; }
        [Column("processed_by")]
        public Guid? ProcessedBy { get; set; }
        [Column("reason")]
        public string Reason { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid GuildDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_GuildJoinRequest_guild_id </summary>
        public virtual GuildDefinitionEntity GuildDefinition { get; set; }
        /// <summary> Relation Label: FK_GuildJoinRequest_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}