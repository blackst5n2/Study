using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("Container")]
    public class ContainerEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("container_type")]
        public ContainerType ContainerType { get; set; }
        [Column("owner_id")]
        public Guid OwnerId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("slot_count")]
        public int SlotCount { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ItemInstance_container_id </summary>
        public virtual ICollection<ItemInstanceEntity> ItemInstances { get; set; } = new HashSet<ItemInstanceEntity>();
        #endregion
    }
}