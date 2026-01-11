using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerSkill
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid SkillDefinitionId { get; set; }
        public int Level { get; set; }
        public bool IsLearned { get; set; }
        public DateTime? AcquiredAt { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerSkill_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerSkill_skill_definition_id </summary>
        public virtual SkillDefinition SkillDefinition { get; set; }
        #endregion
    }
}