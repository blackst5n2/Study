using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class GuildTitle
    {
        public Guid Id { get; set; }
        public Guid GuildId { get; set; }
        public Guid TitleId { get; set; }
        public DateTime AcquiredAt { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GuildTitle_guild_id </summary>
        public virtual GuildDefinition GuildDefinition { get; set; }
        /// <summary> Relation Label: FK_GuildTitle_title_id </summary>
        public virtual TitleDefinition TitleDefinition { get; set; }
        #endregion
    }
}