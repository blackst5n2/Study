using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerPet")]
    public class PlayerPetEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("pet_definition_id")]
        public Guid PetDefinitionId { get; set; }
        [Column("nickname")]
        public string Nickname { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("exp")]
        public int Exp { get; set; }
        [Column("acquired_at")]
        public DateTime AcquiredAt { get; set; }
        [Column("is_summoned")]
        public bool IsSummoned { get; set; }
        [Column("is_locked")]
        public bool IsLocked { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerPet_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerPet_pet_definition_id </summary>
        public virtual PetDefinitionEntity PetDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerPetSkill_player_pet_id </summary>
        public virtual ICollection<PlayerPetSkillEntity> PlayerPetSkills { get; set; } = new HashSet<PlayerPetSkillEntity>();
        #endregion
    }
}