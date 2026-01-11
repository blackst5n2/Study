using Server.Infrastructure.Entities.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerPetSkill")]
    public class PlayerPetSkillEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_pet_id")]
        public Guid PlayerPetId { get; set; }
        [Column("pet_skill_id")]
        public Guid PetSkillId { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("acquired_at")]
        public DateTime AcquiredAt { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerPetSkill_player_pet_id </summary>
        public virtual PlayerPetEntity PlayerPet { get; set; }
        /// <summary> Relation Label: FK_PlayerPetSkill_pet_skill_id </summary>
        public virtual PetSkillEntity PetSkill { get; set; }
        #endregion
    }
}