
namespace Server.Core.Entities.Definitions
{
    public class MonsterAiFsmDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FsmJson { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MonsterDefinition_fsm_id </summary>
        public virtual ICollection<MonsterDefinition> MonsterDefinitions { get; set; } = new HashSet<MonsterDefinition>();
        #endregion
    }
}