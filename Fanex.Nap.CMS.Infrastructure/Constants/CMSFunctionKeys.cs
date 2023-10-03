// --------------------------------------------------------------------------
// <copyright file="CMSFunctionKeys.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure
{
    /// <summary>
    /// CMS Function keys.
    /// </summary>
    public static class CMSFunctionKeys
    {
        public const string Administration = "Administration";

        public const string AssignFunctionToGroup = "AssignFunctionToGroup";

        public const string AssignFunctionToUser = "AssignFunctionToUser";

        public const string AssignSiteToUser = "AssignSiteToUser";

        public const string CloseOpenMembers = "Close_OpenMembers";

        public const string ConfigureCustomerAccess = "ConfigureCustomerAccess";

        public const string ControlTOByEvent = "ControlTOByEvent";

        public const string ControlTOBySMA = "ControlTOBySMA";

        public const string ControlTOBySMAEvent = "ControlTOBySMA_Event";

        public const string ControlTurnover = "ControlTurnover";

        public const string Customer = "Customer";

        public const string LossControl = "LossControl";

        public const string LossMonitor = "LossMonitor";

        public const string ManageFunction = "ManageFunction";

        public const string ManageGroup = "ManageGroup";

        public const string ManageMenu = "ManageMenu";

        public const string ManageRobots = "ManageRobots";

        public const string ManageSmartPunters = "ManageSmartPunters";

        public const string ManageUser = "ManageUser";

        public const string SystemConfiguration = "SystemConfiguration";

        public const string TurnoverUsage = "TurnoverUsage";

        public const string CreditCollection = "CreditCollection";

        // Turnover Control
        public const string GlobalSettingSoccer = "GlobalSetting_Soccer";
        public const string GlobalSettingOtherSports = "GlobalSetting_OtherSports";
        public const string TurnoverMonitor = "TurnoverMonitor";
        public const string TurnoverMonitorSoccer = "TurnoverMonitor_Soccer";
        public const string TurnoverMonitorOtherSports = "TurnoverMonitor_OtherSports";

        // Notification Setting
        public const string TurnoverControlNotification = "TurnoverControlNotification";

        // Win Limit Control
        public const string CurrencyWinLimitSetting = "CurrencyWinLimitSetting";
        public const string PlayerWinLimitManagement = "PlayerWinLimitManagement";
        public const string WinLimitMonitor = "WinLimitMonitor";

        // Payout Management
        public const string PayoutManagement = "PayoutManagement";
        public const string PayoutForBopingLicensees = "PayoutForBopingLicensees";
        public const string PayoutForONEworksLicensees = "PayoutForONEworksLicensees";
        public const string PayoutForCredit = "PayoutForCredit";
        public const string PayoutViewWinningCode = "ViewWinningCode";

        // ColossusBets - Insert Bonus Ticket
        public const string ColossusBetsInsertBonusTicket = "InsertColossusBetsBonusTicket";

        // Virtual Setting Control
        public const string VirtualSettingMonitor = "VirtualSettingMonitor";
        public const string AutoAdjustmentReport = "AutoAdjustmentReport";
        public const string VirtualSettingMonitorAdvance = "VirtualSettingMonitorAdvance";
        public const string ESports_VirtualRateDetails = "E_Sports_VirtualRateDetails";
        public const string Soccer_VirtualRateDetails = "Soccer_VirtualRateDetails";
        public const string ESports_VirtualRateDetails_GlobalSetting = "E_Sports_VirtualRateDetails_GlobalSetting";
        public const string Soccer_VirtualRateDetails_GlobalSetting = "Soccer_VirtualRateDetails_GlobalSetting";
        public const string VirtualRateLog = "VirtualRateLog";
        public const string Soccer_VirtualRateLog = "Soccer_VirtualRateLog";
        public const string AutoSuspendLog = "AutoSuspendLog";
        public const string ESports_GlobalAutoSuspendThreshold = "E_Sports_GlobalAutoSuspendThreshold";
        public const string ESports_ManuallyUpdatedVirtualRateAccounts = "E_Sports_ManuallyUpdatedVirtualRateAccounts";
        public const string ESports_UploadVirtualRate = "E_Sports_UploadVirtualRate";
        public const string Basketball_VirtualRateDetails = "Basketball_VirtualRateDetails";
        public const string Basketball_VirtualRateDetails_GlobalSetting = "Basketball_VirtualRateDetails_GlobalSetting";
        public const string Basketball_VirtualRateLog = "Basketball_VirtualRateLog";
        public const string Basketball_AutoSuspendLog = "Basketball_AutoSuspendLog";
        public const string Basketball_GlobalAutoSuspendThreshold = "Basketball_GlobalAutoSuspendThreshold";
        public const string Basketball_ManuallyUpdatedVRAccounts = "Basketball_ManuallyUpdatedVRAccounts";
        public const string Basketball_UploadVirtualRate = "Basketball_UploadVirtualRate";

        public const string ManageRobots2 = "ManageRobots2";
        public const string BetRestriction_Soccer = "BetRestriction_Soccer";
        public const string BetRestriction_BasketBall = "BetRestriction_BasketBall";
        public const string BetRestriction_Badminton = "BetRestriction_Badminton";
        public const string BetRestriction_Tennis = "BetRestriction_Tennis";
        public const string OnOffBetRestriction = "OnOffBetRestriction";

        public const string BetLimitTypeControl = "BetLimitTypeControl";
        public const string BetLimitControl = "BetLimitControl";
        public const string DefaultMemberBetLimit = "DefaultBetLimitforNewMember";

        // Problem Account
        public const string ManageProblemAccountCN88 = "ManageProblemAccount_CN88";
        public const string ManageProblemAccountCreditSites = "ManageProblemAccount_CreditSites";
        public const string ManageProblemAccountLicenseeSites = "ManageProblemAccount_LicenseeSites";
        public const string ProblemAccountManageLicenseeAcknowledgement = "ProblemAccount_ManageLicenseeAcknowledgement";

        // Limit Daily PT Update
        public const string LimitDailyPTUpdate = "LimitDailyPTUpdate";

        // Limit New Downline Per Day
        public const string LimitNewDownlinePerDay = "LimitNewDownlinePerDay";

        // Whitelist Big Credit Members
        public const string WhitelistBigCreditMembers = "WhitelistBigCreditMembers";

        // Virtual Rate Engine
        public const string VirtualRateEngine = "VirtualRateEngine";
        public const string VirtualRateEngineAdminPermission = "AdminPermission";
        public const string VirtualRateEngineUserPermission = "UserPermission";

        // Manage Chatbots
        public const string ChatbotHistory = "ChatbotHistory";
        public const string ChatbotMapping = "ChatbotMapping";
        public const string NotificationList = "NotificationList";

        //Accounting site
        public const string SpecialSites = "[AccessSpecialSites]";

        // ManageFollowBetAccount
        public const string ManageFollowBetAccount = "ManageFollowBetAccount";
        
        // Customer Class Management
        public const string CustomerClassSetting = "CustomerClassSetting";
        public const string RuleManagement = "RuleManagement";
        public const string PolicyManagement = "PolicyManagement";
        public const string SiteManagement = "SiteManagement";
        public const string SpreadManagement = "SpreadManagement";
        
        
        //Manage Product Setting
        public const string ManageProductSetting = "ManageProductSetting";
        
        public const string CCUtilityOffering = "CCUtilityOffering";

        // Admin
        public const string ConcurrentUserReport = "ConcurrentUserReport";
    }
}
