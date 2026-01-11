using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class GuildNotice
    {
        public Guid Id { get; set; }
        public Guid GuildId { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPinned { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GuildNotice_guild_id </summary>
        public virtual GuildDefinition GuildDefinition { get; set; }
        /// <summary> Relation Label: FK_GuildNotice_author_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}