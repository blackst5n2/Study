using Server.Core.Entities.Definitions;
using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Refs;
using Server.Core.Entities.UserContents;

namespace Server.Core.Entities.Progress
{
    public class Player
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Code { get; set; }
        public string Nickname { get; set; }
        public int Level { get; set; }
        public long Experience { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogoutAt { get; set; }
        public int PlayTimeSeconds { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Player_account_id </summary>
        public virtual Account Account { get; set; }
        /// <summary> Relation Label: FK_MapPlayerState_player_id </summary>
        public virtual ICollection<MapPlayerState> MapPlayerStates { get; set; } = new HashSet<MapPlayerState>();
        /// <summary> Relation Label: FK_ItemAcquisitionLog_player_id </summary>
        public virtual ICollection<ItemAcquisitionLog> ItemAcquisitionLogs { get; set; } = new HashSet<ItemAcquisitionLog>();
        /// <summary> Relation Label: FK_ItemEnhancementLog_player_id </summary>
        public virtual ICollection<ItemEnhancementLog> ItemEnhancementLogs { get; set; } = new HashSet<ItemEnhancementLog>();
        /// <summary> Relation Label: FK_PlayerInventoryTabLimit_player_id </summary>
        public virtual ICollection<PlayerInventoryTabLimit> PlayerInventoryTabLimits { get; set; } = new HashSet<PlayerInventoryTabLimit>();
        /// <summary> Relation Label: FK_PlayerInventoryItem_player_id </summary>
        public virtual ICollection<PlayerInventoryItem> PlayerInventoryItems { get; set; } = new HashSet<PlayerInventoryItem>();
        /// <summary> Relation Label: FK_PlayerCraftingLog_player_id </summary>
        public virtual ICollection<PlayerCraftingLog> PlayerCraftingLogs { get; set; } = new HashSet<PlayerCraftingLog>();
        /// <summary> Relation Label: FK_PlayerRecipe_player_id </summary>
        public virtual ICollection<PlayerRecipe> PlayerRecipes { get; set; } = new HashSet<PlayerRecipe>();
        /// <summary> Relation Label: FK_PlayerCookingLog_player_id </summary>
        public virtual ICollection<PlayerCookingLog> PlayerCookingLogs { get; set; } = new HashSet<PlayerCookingLog>();
        /// <summary> Relation Label: FK_PortalUseLog_player_id </summary>
        public virtual ICollection<PortalUseLog> PortalUseLogs { get; set; } = new HashSet<PortalUseLog>();
        /// <summary> Relation Label: FK_PlayerCropInstance_player_id </summary>
        public virtual ICollection<PlayerCropInstance> PlayerCropInstances { get; set; } = new HashSet<PlayerCropInstance>();
        /// <summary> Relation Label: FK_PlayerCropHarvestLog_player_id </summary>
        public virtual ICollection<PlayerCropHarvestLog> PlayerCropHarvestLogs { get; set; } = new HashSet<PlayerCropHarvestLog>();
        /// <summary> Relation Label: FK_PlayerFishingLog_player_id </summary>
        public virtual ICollection<PlayerFishingLog> PlayerFishingLogs { get; set; } = new HashSet<PlayerFishingLog>();
        /// <summary> Relation Label: FK_PlayerLivestockInstance_player_id </summary>
        public virtual ICollection<PlayerLivestockInstance> PlayerLivestockInstances { get; set; } = new HashSet<PlayerLivestockInstance>();
        /// <summary> Relation Label: FK_PlayerLivestockProductLog_player_id </summary>
        public virtual ICollection<PlayerLivestockProductLog> PlayerLivestockProductLogs { get; set; } = new HashSet<PlayerLivestockProductLog>();
        /// <summary> Relation Label: FK_PlayerLivestockFeedLog_player_id </summary>
        public virtual ICollection<PlayerLivestockFeedLog> PlayerLivestockFeedLogs { get; set; } = new HashSet<PlayerLivestockFeedLog>();
        /// <summary> Relation Label: FK_PlayerMonsterKillLog_player_id </summary>
        public virtual ICollection<PlayerMonsterKillLog> PlayerMonsterKillLogs { get; set; } = new HashSet<PlayerMonsterKillLog>();
        /// <summary> Relation Label: FK_PlayerMiningLog_player_id </summary>
        public virtual ICollection<PlayerMiningLog> PlayerMiningLogs { get; set; } = new HashSet<PlayerMiningLog>();
        /// <summary> Relation Label: FK_PlayerLoggingLog_player_id </summary>
        public virtual ICollection<PlayerLoggingLog> PlayerLoggingLogs { get; set; } = new HashSet<PlayerLoggingLog>();
        /// <summary> Relation Label: FK_PlayerLevelLog_player_id </summary>
        public virtual ICollection<PlayerLevelLog> PlayerLevelLogs { get; set; } = new HashSet<PlayerLevelLog>();
        /// <summary> Relation Label: FK_PlayerStatLog_player_id </summary>
        public virtual ICollection<PlayerStatLog> PlayerStatLogs { get; set; } = new HashSet<PlayerStatLog>();
        /// <summary> Relation Label: FK_PlayerMiniGameResult_player_id </summary>
        public virtual ICollection<PlayerMiniGameResult> PlayerMiniGameResults { get; set; } = new HashSet<PlayerMiniGameResult>();
        /// <summary> Relation Label: FK_PlayerJobSkill_player_id </summary>
        public virtual ICollection<PlayerJobSkill> PlayerJobSkills { get; set; } = new HashSet<PlayerJobSkill>();
        /// <summary> Relation Label: FK_PlayerJobSkillPoint_player_id </summary>
        public virtual ICollection<PlayerJobSkillPoint> PlayerJobSkillPoints { get; set; } = new HashSet<PlayerJobSkillPoint>();
        /// <summary> Relation Label: FK_PlayerSkill_player_id </summary>
        public virtual ICollection<PlayerSkill> PlayerSkills { get; set; } = new HashSet<PlayerSkill>();
        /// <summary> Relation Label: FK_PlayerSkillLog_player_id </summary>
        public virtual ICollection<PlayerSkillLog> PlayerSkillLogs { get; set; } = new HashSet<PlayerSkillLog>();
        /// <summary> Relation Label: FK_PlayerQuickSlotPreset_player_id </summary>
        public virtual ICollection<PlayerQuickSlotPreset> PlayerQuickSlotPresets { get; set; } = new HashSet<PlayerQuickSlotPreset>();
        /// <summary> Relation Label: FK_PlayerClass_player_id </summary>
        public virtual ICollection<PlayerClass> PlayerClasses { get; set; } = new HashSet<PlayerClass>();
        /// <summary> Relation Label: FK_PlayerClassChangeLog_player_id </summary>
        public virtual ICollection<PlayerClassChangeLog> PlayerClassChangeLogs { get; set; } = new HashSet<PlayerClassChangeLog>();
        /// <summary> Relation Label: FK_PlayerSkillChangeLog_player_id </summary>
        public virtual ICollection<PlayerSkillChangeLog> PlayerSkillChangeLogs { get; set; } = new HashSet<PlayerSkillChangeLog>();
        /// <summary> Relation Label: FK_ChatLog_sender_id </summary>
        public virtual ICollection<ChatLog> ChatLogs { get; set; } = new HashSet<ChatLog>();
        /// <summary> Relation Label: FK_CommunityReportLog_reporter_id </summary>
        public virtual ICollection<CommunityReportLog> CommunityReportLogs { get; set; } = new HashSet<CommunityReportLog>();
        /// <summary> Relation Label: FK_ModerationActionLog_target_player_id </summary>
        public virtual ICollection<ModerationActionLog> ModerationActionLogs { get; set; } = new HashSet<ModerationActionLog>();
        /// <summary> Relation Label: FK_FriendRelation_player_id_1 </summary>
        public virtual ICollection<FriendRelation> FriendRelations { get; set; } = new HashSet<FriendRelation>();
        /// <summary> Relation Label: FK_PartyDefinition_leader_id </summary>
        public virtual ICollection<PartyDefinition> PartyDefinitions { get; set; } = new HashSet<PartyDefinition>();
        /// <summary> Relation Label: FK_PartyMember_player_id </summary>
        public virtual ICollection<PartyMember> PartyMembers { get; set; } = new HashSet<PartyMember>();
        /// <summary> Relation Label: FK_CommunityNotice_author_id </summary>
        public virtual ICollection<CommunityNotice> CommunityNotices { get; set; } = new HashSet<CommunityNotice>();
        /// <summary> Relation Label: FK_GuildDefinition_leader_id </summary>
        public virtual ICollection<GuildDefinition> GuildDefinitions { get; set; } = new HashSet<GuildDefinition>();
        /// <summary> Relation Label: FK_GuildMember_player_id </summary>
        public virtual ICollection<GuildMember> GuildMembers { get; set; } = new HashSet<GuildMember>();
        /// <summary> Relation Label: FK_GuildNotice_author_id </summary>
        public virtual ICollection<GuildNotice> GuildNotices { get; set; } = new HashSet<GuildNotice>();
        /// <summary> Relation Label: FK_GuildJoinRequest_player_id </summary>
        public virtual ICollection<GuildJoinRequest> GuildJoinRequests { get; set; } = new HashSet<GuildJoinRequest>();
        /// <summary> Relation Label: FK_GuildContributionLog_player_id </summary>
        public virtual ICollection<GuildContributionLog> GuildContributionLogs { get; set; } = new HashSet<GuildContributionLog>();
        /// <summary> Relation Label: FK_Recommendation_from_player_id </summary>
        public virtual ICollection<Recommendation> Recommendations { get; set; } = new HashSet<Recommendation>();
        /// <summary> Relation Label: FK_RecommendationLog_actor_player_id </summary>
        public virtual ICollection<RecommendationLog> RecommendationLogs { get; set; } = new HashSet<RecommendationLog>();
        /// <summary> Relation Label: FK_PlayerTitle_player_id </summary>
        public virtual ICollection<PlayerTitle> PlayerTitles { get; set; } = new HashSet<PlayerTitle>();
        /// <summary> Relation Label: FK_PlayerTitleSlot_player_id </summary>
        public virtual ICollection<PlayerTitleSlot> PlayerTitleSlots { get; set; } = new HashSet<PlayerTitleSlot>();
        /// <summary> Relation Label: FK_TitleUnlockHistory_player_id </summary>
        public virtual ICollection<TitleUnlockHistory> TitleUnlockHistories { get; set; } = new HashSet<TitleUnlockHistory>();
        /// <summary> Relation Label: FK_Notification_player_id </summary>
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        /// <summary> Relation Label: FK_PlayerBuff_player_id </summary>
        public virtual ICollection<PlayerBuff> PlayerBuffs { get; set; } = new HashSet<PlayerBuff>();
        /// <summary> Relation Label: FK_PlayerBuffLog_player_id </summary>
        public virtual ICollection<PlayerBuffLog> PlayerBuffLogs { get; set; } = new HashSet<PlayerBuffLog>();
        /// <summary> Relation Label: FK_PlayerPet_player_id </summary>
        public virtual ICollection<PlayerPet> PlayerPets { get; set; } = new HashSet<PlayerPet>();
        /// <summary> Relation Label: FK_PlayerEventProgress_player_id </summary>
        public virtual ICollection<PlayerEventProgress> PlayerEventProgresses { get; set; } = new HashSet<PlayerEventProgress>();
        /// <summary> Relation Label: FK_PlayerEventRewardLog_player_id </summary>
        public virtual ICollection<PlayerEventRewardLog> PlayerEventRewardLogs { get; set; } = new HashSet<PlayerEventRewardLog>();
        /// <summary> Relation Label: FK_PlayerSeasonPass_player_id </summary>
        public virtual ICollection<PlayerSeasonPass> PlayerSeasonPasses { get; set; } = new HashSet<PlayerSeasonPass>();
        /// <summary> Relation Label: FK_PlayerSeasonPassRewardLog_player_id </summary>
        public virtual ICollection<PlayerSeasonPassRewardLog> PlayerSeasonPassRewardLogs { get; set; } = new HashSet<PlayerSeasonPassRewardLog>();
        /// <summary> Relation Label: FK_SeasonPassPurchaseLog_player_id </summary>
        public virtual ICollection<SeasonPassPurchaseLog> SeasonPassPurchaseLogs { get; set; } = new HashSet<SeasonPassPurchaseLog>();
        /// <summary> Relation Label: FK_SeasonPassMissionLog_player_id </summary>
        public virtual ICollection<SeasonPassMissionLog> SeasonPassMissionLogs { get; set; } = new HashSet<SeasonPassMissionLog>();
        /// <summary> Relation Label: FK_PlayerNotificationLog_player_id </summary>
        public virtual ICollection<PlayerNotificationLog> PlayerNotificationLogs { get; set; } = new HashSet<PlayerNotificationLog>();
        /// <summary> Relation Label: FK_PlayerBuilding_player_id </summary>
        public virtual ICollection<PlayerBuilding> PlayerBuildings { get; set; } = new HashSet<PlayerBuilding>();
        /// <summary> Relation Label: FK_BuildingActionLog_player_id </summary>
        public virtual ICollection<BuildingActionLog> BuildingActionLogs { get; set; } = new HashSet<BuildingActionLog>();
        /// <summary> Relation Label: FK_MarketListing_seller_id </summary>
        public virtual ICollection<MarketListing> MarketListings { get; set; } = new HashSet<MarketListing>();
        /// <summary> Relation Label: FK_MarketTransaction_buyer_id </summary>
        public virtual ICollection<MarketTransaction> MarketTransactions { get; set; } = new HashSet<MarketTransaction>();
        /// <summary> Relation Label: FK_Trade_player_1_id </summary>
        public virtual ICollection<Trade> Trades { get; set; } = new HashSet<Trade>();
        /// <summary> Relation Label: FK_TradeItem_offering_player_id </summary>
        public virtual ICollection<TradeItem> TradeItems { get; set; } = new HashSet<TradeItem>();
        /// <summary> Relation Label: FK_CurrencyTransactionLog_player_id </summary>
        public virtual ICollection<CurrencyTransactionLog> CurrencyTransactionLogs { get; set; } = new HashSet<CurrencyTransactionLog>();
        /// <summary> Relation Label: FK_BoardGameRoom_host_player_id </summary>
        public virtual ICollection<BoardGameRoom> BoardGameRooms { get; set; } = new HashSet<BoardGameRoom>();
        /// <summary> Relation Label: FK_BoardGameParticipant_player_id </summary>
        public virtual ICollection<BoardGameParticipant> BoardGameParticipants { get; set; } = new HashSet<BoardGameParticipant>();
        /// <summary> Relation Label: FK_BoardGameTurnLog_player_id </summary>
        public virtual ICollection<BoardGameTurnLog> BoardGameTurnLogs { get; set; } = new HashSet<BoardGameTurnLog>();
        /// <summary> Relation Label: FK_UserPattern_creator_id </summary>
        public virtual ICollection<UserPattern> UserPatterns { get; set; } = new HashSet<UserPattern>();
        /// <summary> Relation Label: FK_UserPatternLike_player_id </summary>
        public virtual ICollection<UserPatternLike> UserPatternLikes { get; set; } = new HashSet<UserPatternLike>();
        /// <summary> Relation Label: FK_UserPatternComment_player_id </summary>
        public virtual ICollection<UserPatternComment> UserPatternComments { get; set; } = new HashSet<UserPatternComment>();
        /// <summary> Relation Label: FK_UserRoom_owner_id </summary>
        public virtual ICollection<UserRoom> UserRooms { get; set; } = new HashSet<UserRoom>();
        /// <summary> Relation Label: FK_UserRoomParticipant_player_id </summary>
        public virtual ICollection<UserRoomParticipant> UserRoomParticipants { get; set; } = new HashSet<UserRoomParticipant>();
        /// <summary> Relation Label: FK_UserRoomChat_player_id </summary>
        public virtual ICollection<UserRoomChat> UserRoomChats { get; set; } = new HashSet<UserRoomChat>();
        /// <summary> Relation Label: FK_UserContentReport_reporter_id </summary>
        public virtual ICollection<UserContentReport> UserContentReports { get; set; } = new HashSet<UserContentReport>();
        /// <summary> Relation Label: FK_UserRoomFavorite_player_id </summary>
        public virtual ICollection<UserRoomFavorite> UserRoomFavorites { get; set; } = new HashSet<UserRoomFavorite>();
        /// <summary> Relation Label: FK_UserPatternFavorite_player_id </summary>
        public virtual ICollection<UserPatternFavorite> UserPatternFavorites { get; set; } = new HashSet<UserPatternFavorite>();
        /// <summary> Relation Label: FK_UserPatternBoardPost_creator_id </summary>
        public virtual ICollection<UserPatternBoardPost> UserPatternBoardPosts { get; set; } = new HashSet<UserPatternBoardPost>();
        /// <summary> Relation Label: FK_UserPatternBoardPostComment_commenter_id </summary>
        public virtual ICollection<UserPatternBoardPostComment> UserPatternBoardPostComments { get; set; } = new HashSet<UserPatternBoardPostComment>();
        /// <summary> Relation Label: FK_DungeonRun_leader_id </summary>
        public virtual ICollection<DungeonRun> DungeonRuns { get; set; } = new HashSet<DungeonRun>();
        /// <summary> Relation Label: FK_DungeonRunParticipant_player_id </summary>
        public virtual ICollection<DungeonRunParticipant> DungeonRunParticipants { get; set; } = new HashSet<DungeonRunParticipant>();
        /// <summary> Relation Label: FK_SecurityIncidentLog_player_id </summary>
        public virtual ICollection<SecurityIncidentLog> SecurityIncidentLogs { get; set; } = new HashSet<SecurityIncidentLog>();
        /// <summary> Relation Label: FK_PlayerBan_player_id </summary>
        public virtual ICollection<PlayerBan> PlayerBans { get; set; } = new HashSet<PlayerBan>();
        /// <summary> Relation Label: FK_PlayerWorldEventParticipation_player_id </summary>
        public virtual ICollection<PlayerWorldEventParticipation> PlayerWorldEventParticipations { get; set; } = new HashSet<PlayerWorldEventParticipation>();
        /// <summary> Relation Label: FK_PlayerShopPurchaseLimit_player_id </summary>
        public virtual ICollection<PlayerShopPurchaseLimit> PlayerShopPurchaseLimits { get; set; } = new HashSet<PlayerShopPurchaseLimit>();
        /// <summary> Relation Label: FK_ShopTransactionLog_player_id </summary>
        public virtual ICollection<ShopTransactionLog> ShopTransactionLogs { get; set; } = new HashSet<ShopTransactionLog>();
        /// <summary> Relation Label: FK_PlayerNpcFavor_player_id </summary>
        public virtual ICollection<PlayerNpcFavor> PlayerNpcFavors { get; set; } = new HashSet<PlayerNpcFavor>();
        /// <summary> Relation Label: FK_QuestLog_player_id </summary>
        public virtual ICollection<QuestLog> QuestLogs { get; set; } = new HashSet<QuestLog>();
        /// <summary> Relation Label: FK_AchievementLog_player_id </summary>
        public virtual ICollection<AchievementLog> AchievementLogs { get; set; } = new HashSet<AchievementLog>();
        #endregion
    }
}