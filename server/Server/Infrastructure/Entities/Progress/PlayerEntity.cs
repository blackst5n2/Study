using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Refs;
using Server.Infrastructure.Entities.UserContents;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("Player")]
    public class PlayerEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("account_id")]
        public Guid AccountId { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("nickname")]
        public string Nickname { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("experience")]
        public long Experience { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("last_logout_at")]
        public DateTime LastLogoutAt { get; set; }
        [Column("play_time_seconds")]
        public int PlayTimeSeconds { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Player_account_id </summary>
        public virtual AccountEntity Account { get; set; }
        /// <summary> Relation Label: FK_MapPlayerState_player_id </summary>
        public virtual ICollection<MapPlayerStateEntity> MapPlayerStates { get; set; } = new HashSet<MapPlayerStateEntity>();
        /// <summary> Relation Label: FK_ItemAcquisitionLog_player_id </summary>
        public virtual ICollection<ItemAcquisitionLogEntity> ItemAcquisitionLogs { get; set; } = new HashSet<ItemAcquisitionLogEntity>();
        /// <summary> Relation Label: FK_ItemEnhancementLog_player_id </summary>
        public virtual ICollection<ItemEnhancementLogEntity> ItemEnhancementLogs { get; set; } = new HashSet<ItemEnhancementLogEntity>();
        /// <summary> Relation Label: FK_PlayerInventoryTabLimit_player_id </summary>
        public virtual ICollection<PlayerInventoryTabLimitEntity> PlayerInventoryTabLimits { get; set; } = new HashSet<PlayerInventoryTabLimitEntity>();
        /// <summary> Relation Label: FK_PlayerInventoryItem_player_id </summary>
        public virtual ICollection<PlayerInventoryItemEntity> PlayerInventoryItems { get; set; } = new HashSet<PlayerInventoryItemEntity>();
        /// <summary> Relation Label: FK_PlayerCraftingLog_player_id </summary>
        public virtual ICollection<PlayerCraftingLogEntity> PlayerCraftingLogs { get; set; } = new HashSet<PlayerCraftingLogEntity>();
        /// <summary> Relation Label: FK_PlayerRecipe_player_id </summary>
        public virtual ICollection<PlayerRecipeEntity> PlayerRecipes { get; set; } = new HashSet<PlayerRecipeEntity>();
        /// <summary> Relation Label: FK_PlayerCookingLog_player_id </summary>
        public virtual ICollection<PlayerCookingLogEntity> PlayerCookingLogs { get; set; } = new HashSet<PlayerCookingLogEntity>();
        /// <summary> Relation Label: FK_PortalUseLog_player_id </summary>
        public virtual ICollection<PortalUseLogEntity> PortalUseLogs { get; set; } = new HashSet<PortalUseLogEntity>();
        /// <summary> Relation Label: FK_PlayerCropInstance_player_id </summary>
        public virtual ICollection<PlayerCropInstanceEntity> PlayerCropInstances { get; set; } = new HashSet<PlayerCropInstanceEntity>();
        /// <summary> Relation Label: FK_PlayerCropHarvestLog_player_id </summary>
        public virtual ICollection<PlayerCropHarvestLogEntity> PlayerCropHarvestLogs { get; set; } = new HashSet<PlayerCropHarvestLogEntity>();
        /// <summary> Relation Label: FK_PlayerFishingLog_player_id </summary>
        public virtual ICollection<PlayerFishingLogEntity> PlayerFishingLogs { get; set; } = new HashSet<PlayerFishingLogEntity>();
        /// <summary> Relation Label: FK_PlayerLivestockInstance_player_id </summary>
        public virtual ICollection<PlayerLivestockInstanceEntity> PlayerLivestockInstances { get; set; } = new HashSet<PlayerLivestockInstanceEntity>();
        /// <summary> Relation Label: FK_PlayerLivestockProductLog_player_id </summary>
        public virtual ICollection<PlayerLivestockProductLogEntity> PlayerLivestockProductLogs { get; set; } = new HashSet<PlayerLivestockProductLogEntity>();
        /// <summary> Relation Label: FK_PlayerLivestockFeedLog_player_id </summary>
        public virtual ICollection<PlayerLivestockFeedLogEntity> PlayerLivestockFeedLogs { get; set; } = new HashSet<PlayerLivestockFeedLogEntity>();
        /// <summary> Relation Label: FK_PlayerMonsterKillLog_player_id </summary>
        public virtual ICollection<PlayerMonsterKillLogEntity> PlayerMonsterKillLogs { get; set; } = new HashSet<PlayerMonsterKillLogEntity>();
        /// <summary> Relation Label: FK_PlayerMiningLog_player_id </summary>
        public virtual ICollection<PlayerMiningLogEntity> PlayerMiningLogs { get; set; } = new HashSet<PlayerMiningLogEntity>();
        /// <summary> Relation Label: FK_PlayerLoggingLog_player_id </summary>
        public virtual ICollection<PlayerLoggingLogEntity> PlayerLoggingLogs { get; set; } = new HashSet<PlayerLoggingLogEntity>();
        /// <summary> Relation Label: FK_PlayerLevelLog_player_id </summary>
        public virtual ICollection<PlayerLevelLogEntity> PlayerLevelLogs { get; set; } = new HashSet<PlayerLevelLogEntity>();
        /// <summary> Relation Label: FK_PlayerStatLog_player_id </summary>
        public virtual ICollection<PlayerStatLogEntity> PlayerStatLogs { get; set; } = new HashSet<PlayerStatLogEntity>();
        /// <summary> Relation Label: FK_PlayerMiniGameResult_player_id </summary>
        public virtual ICollection<PlayerMiniGameResultEntity> PlayerMiniGameResults { get; set; } = new HashSet<PlayerMiniGameResultEntity>();
        /// <summary> Relation Label: FK_PlayerJobSkill_player_id </summary>
        public virtual ICollection<PlayerJobSkillEntity> PlayerJobSkills { get; set; } = new HashSet<PlayerJobSkillEntity>();
        /// <summary> Relation Label: FK_PlayerJobSkillPoint_player_id </summary>
        public virtual ICollection<PlayerJobSkillPointEntity> PlayerJobSkillPoints { get; set; } = new HashSet<PlayerJobSkillPointEntity>();
        /// <summary> Relation Label: FK_PlayerSkill_player_id </summary>
        public virtual ICollection<PlayerSkillEntity> PlayerSkills { get; set; } = new HashSet<PlayerSkillEntity>();
        /// <summary> Relation Label: FK_PlayerSkillLog_player_id </summary>
        public virtual ICollection<PlayerSkillLogEntity> PlayerSkillLogs { get; set; } = new HashSet<PlayerSkillLogEntity>();
        /// <summary> Relation Label: FK_PlayerQuickSlotPreset_player_id </summary>
        public virtual ICollection<PlayerQuickSlotPresetEntity> PlayerQuickSlotPresets { get; set; } = new HashSet<PlayerQuickSlotPresetEntity>();
        /// <summary> Relation Label: FK_PlayerClass_player_id </summary>
        public virtual ICollection<PlayerClassEntity> PlayerClasses { get; set; } = new HashSet<PlayerClassEntity>();
        /// <summary> Relation Label: FK_PlayerClassChangeLog_player_id </summary>
        public virtual ICollection<PlayerClassChangeLogEntity> PlayerClassChangeLogs { get; set; } = new HashSet<PlayerClassChangeLogEntity>();
        /// <summary> Relation Label: FK_PlayerSkillChangeLog_player_id </summary>
        public virtual ICollection<PlayerSkillChangeLogEntity> PlayerSkillChangeLogs { get; set; } = new HashSet<PlayerSkillChangeLogEntity>();
        /// <summary> Relation Label: FK_ChatLog_sender_id </summary>
        public virtual ICollection<ChatLogEntity> ChatLogs { get; set; } = new HashSet<ChatLogEntity>();
        /// <summary> Relation Label: FK_CommunityReportLog_reporter_id </summary>
        public virtual ICollection<CommunityReportLogEntity> CommunityReportLogs { get; set; } = new HashSet<CommunityReportLogEntity>();
        /// <summary> Relation Label: FK_ModerationActionLog_target_player_id </summary>
        public virtual ICollection<ModerationActionLogEntity> ModerationActionLogs { get; set; } = new HashSet<ModerationActionLogEntity>();
        /// <summary> Relation Label: FK_FriendRelation_player_id_1 </summary>
        public virtual ICollection<FriendRelationEntity> FriendRelations { get; set; } = new HashSet<FriendRelationEntity>();
        /// <summary> Relation Label: FK_PartyDefinition_leader_id </summary>
        public virtual ICollection<PartyDefinitionEntity> PartyDefinitions { get; set; } = new HashSet<PartyDefinitionEntity>();
        /// <summary> Relation Label: FK_PartyMember_player_id </summary>
        public virtual ICollection<PartyMemberEntity> PartyMembers { get; set; } = new HashSet<PartyMemberEntity>();
        /// <summary> Relation Label: FK_CommunityNotice_author_id </summary>
        public virtual ICollection<CommunityNoticeEntity> CommunityNotices { get; set; } = new HashSet<CommunityNoticeEntity>();
        /// <summary> Relation Label: FK_GuildDefinition_leader_id </summary>
        public virtual ICollection<GuildDefinitionEntity> GuildDefinitions { get; set; } = new HashSet<GuildDefinitionEntity>();
        /// <summary> Relation Label: FK_GuildMember_player_id </summary>
        public virtual ICollection<GuildMemberEntity> GuildMembers { get; set; } = new HashSet<GuildMemberEntity>();
        /// <summary> Relation Label: FK_GuildNotice_author_id </summary>
        public virtual ICollection<GuildNoticeEntity> GuildNotices { get; set; } = new HashSet<GuildNoticeEntity>();
        /// <summary> Relation Label: FK_GuildJoinRequest_player_id </summary>
        public virtual ICollection<GuildJoinRequestEntity> GuildJoinRequests { get; set; } = new HashSet<GuildJoinRequestEntity>();
        /// <summary> Relation Label: FK_GuildContributionLog_player_id </summary>
        public virtual ICollection<GuildContributionLogEntity> GuildContributionLogs { get; set; } = new HashSet<GuildContributionLogEntity>();
        /// <summary> Relation Label: FK_Recommendation_from_player_id </summary>
        public virtual ICollection<RecommendationEntity> Recommendations { get; set; } = new HashSet<RecommendationEntity>();
        /// <summary> Relation Label: FK_RecommendationLog_actor_player_id </summary>
        public virtual ICollection<RecommendationLogEntity> RecommendationLogs { get; set; } = new HashSet<RecommendationLogEntity>();
        /// <summary> Relation Label: FK_PlayerTitle_player_id </summary>
        public virtual ICollection<PlayerTitleEntity> PlayerTitles { get; set; } = new HashSet<PlayerTitleEntity>();
        /// <summary> Relation Label: FK_PlayerTitleSlot_player_id </summary>
        public virtual ICollection<PlayerTitleSlotEntity> PlayerTitleSlots { get; set; } = new HashSet<PlayerTitleSlotEntity>();
        /// <summary> Relation Label: FK_TitleUnlockHistory_player_id </summary>
        public virtual ICollection<TitleUnlockHistoryEntity> TitleUnlockHistories { get; set; } = new HashSet<TitleUnlockHistoryEntity>();
        /// <summary> Relation Label: FK_Notification_player_id </summary>
        public virtual ICollection<NotificationEntity> Notifications { get; set; } = new HashSet<NotificationEntity>();
        /// <summary> Relation Label: FK_PlayerBuff_player_id </summary>
        public virtual ICollection<PlayerBuffEntity> PlayerBuffs { get; set; } = new HashSet<PlayerBuffEntity>();
        /// <summary> Relation Label: FK_PlayerBuffLog_player_id </summary>
        public virtual ICollection<PlayerBuffLogEntity> PlayerBuffLogs { get; set; } = new HashSet<PlayerBuffLogEntity>();
        /// <summary> Relation Label: FK_PlayerPet_player_id </summary>
        public virtual ICollection<PlayerPetEntity> PlayerPets { get; set; } = new HashSet<PlayerPetEntity>();
        /// <summary> Relation Label: FK_PlayerEventProgress_player_id </summary>
        public virtual ICollection<PlayerEventProgressEntity> PlayerEventProgresses { get; set; } = new HashSet<PlayerEventProgressEntity>();
        /// <summary> Relation Label: FK_PlayerEventRewardLog_player_id </summary>
        public virtual ICollection<PlayerEventRewardLogEntity> PlayerEventRewardLogs { get; set; } = new HashSet<PlayerEventRewardLogEntity>();
        /// <summary> Relation Label: FK_PlayerSeasonPass_player_id </summary>
        public virtual ICollection<PlayerSeasonPassEntity> PlayerSeasonPasses { get; set; } = new HashSet<PlayerSeasonPassEntity>();
        /// <summary> Relation Label: FK_PlayerSeasonPassRewardLog_player_id </summary>
        public virtual ICollection<PlayerSeasonPassRewardLogEntity> PlayerSeasonPassRewardLogs { get; set; } = new HashSet<PlayerSeasonPassRewardLogEntity>();
        /// <summary> Relation Label: FK_SeasonPassPurchaseLog_player_id </summary>
        public virtual ICollection<SeasonPassPurchaseLogEntity> SeasonPassPurchaseLogs { get; set; } = new HashSet<SeasonPassPurchaseLogEntity>();
        /// <summary> Relation Label: FK_SeasonPassMissionLog_player_id </summary>
        public virtual ICollection<SeasonPassMissionLogEntity> SeasonPassMissionLogs { get; set; } = new HashSet<SeasonPassMissionLogEntity>();
        /// <summary> Relation Label: FK_PlayerNotificationLog_player_id </summary>
        public virtual ICollection<PlayerNotificationLogEntity> PlayerNotificationLogs { get; set; } = new HashSet<PlayerNotificationLogEntity>();
        /// <summary> Relation Label: FK_PlayerBuilding_player_id </summary>
        public virtual ICollection<PlayerBuildingEntity> PlayerBuildings { get; set; } = new HashSet<PlayerBuildingEntity>();
        /// <summary> Relation Label: FK_BuildingActionLog_player_id </summary>
        public virtual ICollection<BuildingActionLogEntity> BuildingActionLogs { get; set; } = new HashSet<BuildingActionLogEntity>();
        /// <summary> Relation Label: FK_MarketListing_seller_id </summary>
        public virtual ICollection<MarketListingEntity> MarketListings { get; set; } = new HashSet<MarketListingEntity>();
        /// <summary> Relation Label: FK_MarketTransaction_buyer_id </summary>
        public virtual ICollection<MarketTransactionEntity> MarketTransactions { get; set; } = new HashSet<MarketTransactionEntity>();
        /// <summary> Relation Label: FK_Trade_player_1_id </summary>
        public virtual ICollection<TradeEntity> Trades { get; set; } = new HashSet<TradeEntity>();
        /// <summary> Relation Label: FK_TradeItem_offering_player_id </summary>
        public virtual ICollection<TradeItemEntity> TradeItems { get; set; } = new HashSet<TradeItemEntity>();
        /// <summary> Relation Label: FK_CurrencyTransactionLog_player_id </summary>
        public virtual ICollection<CurrencyTransactionLogEntity> CurrencyTransactionLogs { get; set; } = new HashSet<CurrencyTransactionLogEntity>();
        /// <summary> Relation Label: FK_BoardGameRoom_host_player_id </summary>
        public virtual ICollection<BoardGameRoomEntity> BoardGameRooms { get; set; } = new HashSet<BoardGameRoomEntity>();
        /// <summary> Relation Label: FK_BoardGameParticipant_player_id </summary>
        public virtual ICollection<BoardGameParticipantEntity> BoardGameParticipants { get; set; } = new HashSet<BoardGameParticipantEntity>();
        /// <summary> Relation Label: FK_BoardGameTurnLog_player_id </summary>
        public virtual ICollection<BoardGameTurnLogEntity> BoardGameTurnLogs { get; set; } = new HashSet<BoardGameTurnLogEntity>();
        /// <summary> Relation Label: FK_UserPattern_creator_id </summary>
        public virtual ICollection<UserPatternEntity> UserPatterns { get; set; } = new HashSet<UserPatternEntity>();
        /// <summary> Relation Label: FK_UserPatternLike_player_id </summary>
        public virtual ICollection<UserPatternLikeEntity> UserPatternLikes { get; set; } = new HashSet<UserPatternLikeEntity>();
        /// <summary> Relation Label: FK_UserPatternComment_player_id </summary>
        public virtual ICollection<UserPatternCommentEntity> UserPatternComments { get; set; } = new HashSet<UserPatternCommentEntity>();
        /// <summary> Relation Label: FK_UserRoom_owner_id </summary>
        public virtual ICollection<UserRoomEntity> UserRooms { get; set; } = new HashSet<UserRoomEntity>();
        /// <summary> Relation Label: FK_UserRoomParticipant_player_id </summary>
        public virtual ICollection<UserRoomParticipantEntity> UserRoomParticipants { get; set; } = new HashSet<UserRoomParticipantEntity>();
        /// <summary> Relation Label: FK_UserRoomChat_player_id </summary>
        public virtual ICollection<UserRoomChatEntity> UserRoomChats { get; set; } = new HashSet<UserRoomChatEntity>();
        /// <summary> Relation Label: FK_UserContentReport_reporter_id </summary>
        public virtual ICollection<UserContentReportEntity> UserContentReports { get; set; } = new HashSet<UserContentReportEntity>();
        /// <summary> Relation Label: FK_UserRoomFavorite_player_id </summary>
        public virtual ICollection<UserRoomFavoriteEntity> UserRoomFavorites { get; set; } = new HashSet<UserRoomFavoriteEntity>();
        /// <summary> Relation Label: FK_UserPatternFavorite_player_id </summary>
        public virtual ICollection<UserPatternFavoriteEntity> UserPatternFavorites { get; set; } = new HashSet<UserPatternFavoriteEntity>();
        /// <summary> Relation Label: FK_UserPatternBoardPost_creator_id </summary>
        public virtual ICollection<UserPatternBoardPostEntity> UserPatternBoardPosts { get; set; } = new HashSet<UserPatternBoardPostEntity>();
        /// <summary> Relation Label: FK_UserPatternBoardPostComment_commenter_id </summary>
        public virtual ICollection<UserPatternBoardPostCommentEntity> UserPatternBoardPostComments { get; set; } = new HashSet<UserPatternBoardPostCommentEntity>();
        /// <summary> Relation Label: FK_DungeonRun_leader_id </summary>
        public virtual ICollection<DungeonRunEntity> DungeonRuns { get; set; } = new HashSet<DungeonRunEntity>();
        /// <summary> Relation Label: FK_DungeonRunParticipant_player_id </summary>
        public virtual ICollection<DungeonRunParticipantEntity> DungeonRunParticipants { get; set; } = new HashSet<DungeonRunParticipantEntity>();
        /// <summary> Relation Label: FK_SecurityIncidentLog_player_id </summary>
        public virtual ICollection<SecurityIncidentLogEntity> SecurityIncidentLogs { get; set; } = new HashSet<SecurityIncidentLogEntity>();
        /// <summary> Relation Label: FK_PlayerBan_player_id </summary>
        public virtual ICollection<PlayerBanEntity> PlayerBans { get; set; } = new HashSet<PlayerBanEntity>();
        /// <summary> Relation Label: FK_PlayerWorldEventParticipation_player_id </summary>
        public virtual ICollection<PlayerWorldEventParticipationEntity> PlayerWorldEventParticipations { get; set; } = new HashSet<PlayerWorldEventParticipationEntity>();
        /// <summary> Relation Label: FK_PlayerShopPurchaseLimit_player_id </summary>
        public virtual ICollection<PlayerShopPurchaseLimitEntity> PlayerShopPurchaseLimits { get; set; } = new HashSet<PlayerShopPurchaseLimitEntity>();
        /// <summary> Relation Label: FK_ShopTransactionLog_player_id </summary>
        public virtual ICollection<ShopTransactionLogEntity> ShopTransactionLogs { get; set; } = new HashSet<ShopTransactionLogEntity>();
        /// <summary> Relation Label: FK_PlayerNpcFavor_player_id </summary>
        public virtual ICollection<PlayerNpcFavorEntity> PlayerNpcFavors { get; set; } = new HashSet<PlayerNpcFavorEntity>();
        /// <summary> Relation Label: FK_QuestLog_player_id </summary>
        public virtual ICollection<QuestLogEntity> QuestLogs { get; set; } = new HashSet<QuestLogEntity>();
        /// <summary> Relation Label: FK_AchievementLog_player_id </summary>
        public virtual ICollection<AchievementLogEntity> AchievementLogs { get; set; } = new HashSet<AchievementLogEntity>();
        #endregion
    }
}