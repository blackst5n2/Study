using Server.Core.Entities.Entities;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Definitions
{
    public class TitleDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public Guid? CategoryId { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_TitleDefinition_category_id </summary>
        public virtual TitleCategory TitleCategory { get; set; }
        /// <summary> Relation Label: FK_TitleEffect_title_id </summary>
        public virtual ICollection<TitleEffect> TitleEffects { get; set; } = new HashSet<TitleEffect>();
        /// <summary> Relation Label: FK_TitleUnlockCondition_title_id </summary>
        public virtual ICollection<TitleUnlockCondition> TitleUnlockConditions { get; set; } = new HashSet<TitleUnlockCondition>();
        /// <summary> Relation Label: FK_PlayerTitle_title_id </summary>
        public virtual ICollection<PlayerTitle> PlayerTitles { get; set; } = new HashSet<PlayerTitle>();
        /// <summary> Relation Label: FK_TitleUnlockHistory_title_id </summary>
        public virtual ICollection<TitleUnlockHistory> TitleUnlockHistories { get; set; } = new HashSet<TitleUnlockHistory>();
        /// <summary> Relation Label: FK_GuildTitle_title_id </summary>
        public virtual ICollection<GuildTitle> GuildTitles { get; set; } = new HashSet<GuildTitle>();
        #endregion
    }
}