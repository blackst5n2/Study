using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class FishHabitat
    {
        public Guid Id { get; set; }
        public Guid FishDefinitionId { get; set; }
        public Guid FishingSpotId { get; set; }
        public float SpawnChance { get; set; }
        public Guid? RequiredBaitItemId { get; set; }
        public TimeOfDayType RequiredTime { get; set; }
        public SeasonType RequiredSeason { get; set; }
        public WeatherType RequiredWeather { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_FishHabitat_fish_definition_id </summary>
        public virtual FishDefinition FishDefinition { get; set; }
        /// <summary> Relation Label: FK_FishHabitat_fishing_spot_id </summary>
        public virtual FishingSpotDefinition FishingSpotDefinition { get; set; }
        /// <summary> Relation Label: FK_FishHabitat_required_bait_item_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}