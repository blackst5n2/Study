using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Refs
{
    public class MapEnvironment
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public float? TemperatureDefault { get; set; }
        public float? TemperatureMin { get; set; }
        public float? TemperatureMax { get; set; }
        public string Bgm { get; set; }
        public string Ambience { get; set; }
        public string LightingProfile { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapEnvironment_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        #endregion
    }
}