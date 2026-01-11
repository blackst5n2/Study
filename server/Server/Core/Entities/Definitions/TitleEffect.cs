using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class TitleEffect
    {
        public Guid Id { get; set; }
        public Guid TitleId { get; set; }
        public TitleEffectType EffectType { get; set; }
        public string Target { get; set; }
        public string Value { get; set; }
        public string Extra { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_TitleEffect_title_id </summary>
        public virtual TitleDefinition TitleDefinition { get; set; }
        #endregion
    }
}