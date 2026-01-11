using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("PetDefinition")]
    public class PetDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("entity_definition_id")]
        public Guid EntityDefinitionId { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("species")]
        public string Species { get; set; }
        [Column("rarity")]
        public PetRarity Rarity { get; set; }
        [Column("type")]
        public PetType Type { get; set; }
        [Column("base_stats")]
        public string BaseStats { get; set; }
        [Column("skill_set")]
        public string SkillSet { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("evolve_to")]
        public Guid? EvolveTo { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("model_asset")]
        public string ModelAsset { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PetDefinition_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerPet_pet_definition_id </summary>
        public virtual ICollection<PlayerPetEntity> PlayerPets { get; set; } = new HashSet<PlayerPetEntity>();
        /// <summary> Relation Label: FK_PetSkill_pet_definition_id </summary>
        public virtual ICollection<PetSkillEntity> PetSkills { get; set; } = new HashSet<PetSkillEntity>();
        public Guid ParentId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PetDefinition_evolve_to </summary>
        public virtual PetDefinitionEntity Parent { get; set; }
        /// <summary> Relation Label: FK_PetDefinition_evolve_to </summary>
        public virtual ICollection<PetDefinitionEntity> Children { get; set; } = new HashSet<PetDefinitionEntity>();
        #endregion
    }
}