using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("GuildLevelDefinition")]
    public class GuildLevelDefinitionEntity
    {
        [Column("level")]
        [Key]
        public int Level { get; set; }
        [Column("required_exp")]
        public long RequiredExp { get; set; }
        [Column("member_limit")]
        public int MemberLimit { get; set; }
        [Column("storage_slots")]
        public int StorageSlots { get; set; }
        [Column("guild_buffs")]
        public string GuildBuffs { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        #endregion
    }
}