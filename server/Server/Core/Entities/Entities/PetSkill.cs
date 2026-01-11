using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class PetSkill
    {
        public Guid Id { get; set; }
        public Guid PetDefinitionId { get; set; }
        public Guid SkillDefinitionId { get; set; }
        public int UnlockLevel { get; set; }
        public bool IsPassive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PetSkill_pet_definition_id </summary>
        public virtual PetDefinition PetDefinition { get; set; }
        /// <summary> Relation Label: FK_PetSkill_skill_definition_id </summary>
        public virtual SkillDefinition SkillDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerPetSkill_pet_skill_id </summary>
        public virtual ICollection<PlayerPetSkill> PlayerPetSkills { get; set; } = new HashSet<PlayerPetSkill>();
        #endregion
    }
}