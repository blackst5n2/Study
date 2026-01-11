using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("PetSkill")]
    public class PetSkillEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("pet_definition_id")]
        public Guid PetDefinitionId { get; set; }
        [Column("skill_definition_id")]
        public Guid SkillDefinitionId { get; set; }
        [Column("unlock_level")]
        public int UnlockLevel { get; set; }
        [Column("is_passive")]
        public bool IsPassive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PetSkill_pet_definition_id </summary>
        public virtual PetDefinitionEntity PetDefinition { get; set; }
        /// <summary> Relation Label: FK_PetSkill_skill_definition_id </summary>
        public virtual SkillDefinitionEntity SkillDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerPetSkill_pet_skill_id </summary>
        public virtual ICollection<PlayerPetSkillEntity> PlayerPetSkills { get; set; } = new HashSet<PlayerPetSkillEntity>();
        #endregion
    }
}