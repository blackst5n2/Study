using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerPet
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid PetDefinitionId { get; set; }
        public string Nickname { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public DateTime AcquiredAt { get; set; }
        public bool IsSummoned { get; set; }
        public bool IsLocked { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerPet_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerPet_pet_definition_id </summary>
        public virtual PetDefinition PetDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerPetSkill_player_pet_id </summary>
        public virtual ICollection<PlayerPetSkill> PlayerPetSkills { get; set; } = new HashSet<PlayerPetSkill>();
        #endregion
    }
}