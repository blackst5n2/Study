using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("TitleDefinition")]
    public class TitleDefinitionEntity
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
        [Column("icon")]
        public string Icon { get; set; }
        [Column("category_id")]
        public Guid? CategoryId { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid TitleCategoryId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_TitleDefinition_category_id </summary>
        public virtual TitleCategoryEntity TitleCategory { get; set; }
        /// <summary> Relation Label: FK_TitleEffect_title_id </summary>
        public virtual ICollection<TitleEffectEntity> TitleEffects { get; set; } = new HashSet<TitleEffectEntity>();
        /// <summary> Relation Label: FK_TitleUnlockCondition_title_id </summary>
        public virtual ICollection<TitleUnlockConditionEntity> TitleUnlockConditions { get; set; } = new HashSet<TitleUnlockConditionEntity>();
        /// <summary> Relation Label: FK_PlayerTitle_title_id </summary>
        public virtual ICollection<PlayerTitleEntity> PlayerTitles { get; set; } = new HashSet<PlayerTitleEntity>();
        /// <summary> Relation Label: FK_TitleUnlockHistory_title_id </summary>
        public virtual ICollection<TitleUnlockHistoryEntity> TitleUnlockHistories { get; set; } = new HashSet<TitleUnlockHistoryEntity>();
        /// <summary> Relation Label: FK_GuildTitle_title_id </summary>
        public virtual ICollection<GuildTitleEntity> GuildTitles { get; set; } = new HashSet<GuildTitleEntity>();
        #endregion
    }
}