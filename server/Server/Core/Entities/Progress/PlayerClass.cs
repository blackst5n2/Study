using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerClass
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid ClassId { get; set; }
        public bool IsMain { get; set; }
        public bool IsUnlocked { get; set; }
        public DateTime? AcquiredAt { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerClass_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerClass_class_id </summary>
        public virtual ClassDefinition ClassDefinition { get; set; }
        #endregion
    }
}