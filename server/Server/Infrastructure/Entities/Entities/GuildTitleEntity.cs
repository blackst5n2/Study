using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("GuildTitle")]
    public class GuildTitleEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("guild_id")]
        public Guid GuildId { get; set; }
        [Column("title_id")]
        public Guid TitleId { get; set; }
        [Column("acquired_at")]
        public DateTime AcquiredAt { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid GuildDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_GuildTitle_guild_id </summary>
        public virtual GuildDefinitionEntity GuildDefinition { get; set; }
        public Guid TitleDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_GuildTitle_title_id </summary>
        public virtual TitleDefinitionEntity TitleDefinition { get; set; }
        #endregion
    }
}