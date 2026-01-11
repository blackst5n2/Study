using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerLivestockInstance
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid LivestockDefinitionId { get; set; }
        public string Nickname { get; set; }
        public ItemGrade Grade { get; set; }
        public DateTime AcquiredAt { get; set; }
        public LivestockGrowthStage CurrentGrowthStage { get; set; }
        public float CurrentGrowthTimeMinutes { get; set; }
        public DateTime? LastFedAt { get; set; }
        public int CurrentNutrition { get; set; }
        public DateTime? LastProductCollectedAt { get; set; }
        public string DiseaseState { get; set; }
        public Guid? RanchBuildingId { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerLivestockInstance_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerLivestockInstance_livestock_definition_id </summary>
        public virtual LivestockDefinition LivestockDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerLivestockProductLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockProductLog> PlayerLivestockProductLogs { get; set; } = new HashSet<PlayerLivestockProductLog>();
        /// <summary> Relation Label: FK_PlayerLivestockFeedLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockFeedLog> PlayerLivestockFeedLogs { get; set; } = new HashSet<PlayerLivestockFeedLog>();
        /// <summary> Relation Label: FK_PlayerLivestockDiseaseLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockDiseaseLog> PlayerLivestockDiseaseLogs { get; set; } = new HashSet<PlayerLivestockDiseaseLog>();
        /// <summary> Relation Label: FK_PlayerLivestockGradeLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockGradeLog> PlayerLivestockGradeLogs { get; set; } = new HashSet<PlayerLivestockGradeLog>();
        /// <summary> Relation Label: FK_PlayerLivestockGrowthLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockGrowthLog> PlayerLivestockGrowthLogs { get; set; } = new HashSet<PlayerLivestockGrowthLog>();
        /// <summary> Relation Label: FK_PlayerBuildingSlot_livestock_instance_id </summary>
        public virtual ICollection<PlayerBuildingSlot> PlayerBuildingSlots { get; set; } = new HashSet<PlayerBuildingSlot>();
        #endregion
    }
}