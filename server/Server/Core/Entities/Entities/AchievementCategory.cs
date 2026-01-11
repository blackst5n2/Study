using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class AchievementCategory
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_AchievementDefinition_category_id </summary>
        public virtual ICollection<AchievementDefinition> AchievementDefinitions { get; set; } = new HashSet<AchievementDefinition>();
        #endregion
    }
}