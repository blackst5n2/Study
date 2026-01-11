using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerJobSkill
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid SkillId { get; set; }
        public int Level { get; set; }
        public DateTime AcquiredAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerJobSkill_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerJobSkill_skill_id </summary>
        public virtual JobSkillDefinition JobSkillDefinition { get; set; }
        #endregion
    }
}