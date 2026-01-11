
namespace Server.Core.Entities.Definitions
{
    public class GuildLevelDefinition
    {
        public int Level { get; set; }
        public long RequiredExp { get; set; }
        public int MemberLimit { get; set; }
        public int StorageSlots { get; set; }
        public string GuildBuffs { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        #endregion
    }
}