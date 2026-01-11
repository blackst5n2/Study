using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class Container
    {
        public Guid Id { get; set; }
        public ContainerType ContainerType { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public int SlotCount { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ItemInstance_container_id </summary>
        public virtual ICollection<ItemInstance> ItemInstances { get; set; } = new HashSet<ItemInstance>();
        #endregion
    }
}