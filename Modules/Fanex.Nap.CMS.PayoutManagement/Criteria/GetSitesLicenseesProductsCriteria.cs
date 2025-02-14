namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    using System.Collections;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;

    public class GetSitesLicenseesProductsCriteria : CriteriaBase
    {
        public GetSitesLicenseesProductsCriteria()
        {
            int userId = 0;
            int payoutControl = 0;

            if (AppSession.CurrentUser != null)
            {
                userId = AppSession.CurrentUser.Id;
                int siteId = NapSettings.Settings.SiteId;
                const int readPolicy = (int)Constant.Policy.Read;

                IUserService userService = new UserService();
                int boping = userService.GetPermission(userId, siteId, CMSFunctionKeys.PayoutForBopingLicensees);
                int oneworks = userService.GetPermission(userId, siteId, CMSFunctionKeys.PayoutForONEworksLicensees);
                int credit = userService.GetPermission(userId, siteId, CMSFunctionKeys.PayoutForCredit);

                if (boping >= readPolicy)
                {
                    payoutControl += 2;
                }

                if (oneworks >= readPolicy)
                {
                    payoutControl += 4;
                }

                if (credit >= readPolicy)
                {
                    payoutControl += 8;
                }
            }

            AdminId = userId;
            PayoutControl = payoutControl;
        }

        public int AdminId { get; set; }
        public int PayoutControl { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_PayoutTrans_SelPermission";
        }

        public override bool IsValid()
            => true;
    }
}
