using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class TitleUnlockCondition
    {
        public Guid Id { get; set; }
        public Guid TitleId { get; set; }
        public int ConditionGroup { get; set; }
        public TitleUnlockType Type { get; set; }
        public string Key { get; set; }
        public TitleUnlockOperator Operator { get; set; }
        public string Value { get; set; }
        public string Extra { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_TitleUnlockCondition_title_id </summary>
        public virtual TitleDefinition TitleDefinition { get; set; }
        #endregion
    }
}