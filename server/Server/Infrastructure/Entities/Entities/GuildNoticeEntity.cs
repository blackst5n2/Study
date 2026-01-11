using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("GuildNotice")]
    public class GuildNoticeEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("guild_id")]
        public Guid GuildId { get; set; }
        [Column("author_id")]
        public Guid AuthorId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("is_pinned")]
        public bool IsPinned { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid GuildDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_GuildNotice_guild_id </summary>
        public virtual GuildDefinitionEntity GuildDefinition { get; set; }
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_GuildNotice_author_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}