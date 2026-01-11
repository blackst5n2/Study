using Server.Core.Entities.Entities;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class PetDefinition
    {
        public Guid Id { get; set; }
        public Guid EntityDefinitionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public PetRarity Rarity { get; set; }
        public PetType Type { get; set; }
        public string BaseStats { get; set; }
        public string SkillSet { get; set; }
        public string Description { get; set; }
        public Guid? EvolveTo { get; set; }
        public string Icon { get; set; }
        public string ModelAsset { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PetDefinition_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerPet_pet_definition_id </summary>
        public virtual ICollection<PlayerPet> PlayerPets { get; set; } = new HashSet<PlayerPet>();
        /// <summary> Relation Label: FK_PetSkill_pet_definition_id </summary>
        public virtual ICollection<PetSkill> PetSkills { get; set; } = new HashSet<PetSkill>();
        /// <summary> Relation Label: FK_PetDefinition_evolve_to </summary>
        public virtual PetDefinition Parent { get; set; }
        /// <summary> Relation Label: FK_PetDefinition_evolve_to </summary>
        public virtual ICollection<PetDefinition> Children { get; set; } = new HashSet<PetDefinition>();
        #endregion
    }
}