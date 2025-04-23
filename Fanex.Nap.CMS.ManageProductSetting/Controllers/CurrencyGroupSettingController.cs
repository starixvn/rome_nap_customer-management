namespace Fanex.Nap.CMS.ManageProductSetting.Controllers
{
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using System.Web.Mvc;
    using Services.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.Nap.CMS.ManageProductSetting.ViewModels.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;

    public class CurrencyGroupSettingController : BaseController
    {
        private readonly ICompanyBetLimitSettingService _service;

        public CurrencyGroupSettingController() : this(null)
        {
        }

        internal CurrencyGroupSettingController(ICompanyBetLimitSettingService service)
        {
            this._service = service ?? new CompanyBetLimitSettingService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Read)]
        public ActionResult Search(bool isCredit)
        {
            var result = _service.GetCurrencyGroups(isCredit);

            return JsonData(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult Update(CurrencyGroup currencyGroup, bool isCredit)
        {
            var result = _service.UpdateCurrencyGroups(currencyGroup, isCredit, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult Insert(CurrencyGroup currencyGroup, bool isCredit)
        {
            var result = _service.InsertCurrencyGroups(currencyGroup, isCredit, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult Delete(int groupId, bool isCredit)
        {
            var result = _service.DeleteCurrencyGroups(groupId, isCredit, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result);
        }
    }
}