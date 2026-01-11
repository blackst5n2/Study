using Server.Core.Entities.Entities;

namespace Server.Core.Entities.Progress
{
    public class PlayerPetSkill
    {
        public Guid Id { get; set; }
        public Guid PlayerPetId { get; set; }
        public Guid PetSkillId { get; set; }
        public int Level { get; set; }
        public DateTime AcquiredAt { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerPetSkill_player_pet_id </summary>
        public virtual PlayerPet PlayerPet { get; set; }
        /// <summary> Relation Label: FK_PlayerPetSkill_pet_skill_id </summary>
        public virtual PetSkill PetSkill { get; set; }
        #endregion
    }
}