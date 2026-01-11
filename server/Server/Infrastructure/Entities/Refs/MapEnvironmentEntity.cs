using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("MapEnvironment")]
    public class MapEnvironmentEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("temperature_default")]
        public float? TemperatureDefault { get; set; }
        [Column("temperature_min")]
        public float? TemperatureMin { get; set; }
        [Column("temperature_max")]
        public float? TemperatureMax { get; set; }
        [Column("bgm")]
        public string Bgm { get; set; }
        [Column("ambience")]
        public string Ambience { get; set; }
        [Column("lighting_profile")]
        public string LightingProfile { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapEnvironment_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        #endregion
    }
}