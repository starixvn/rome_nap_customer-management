namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Controllers;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using ViewModels.BetList;
    using ViewModels.ProblemAccount;

    public class BetListController : BaseController
    {
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Read)]
        public ActionResult AbnormalBetTicket(int custId, string custName, SearchViewModel searchViewModel)
        {
            TempData[ProblemAccountController.SearchFilterSessionKey] = searchViewModel;

            var vm = new AbnormalBetTicketViewModel
            {
                CustId = custId,
                CustName = custName
            };

            return View("~/Areas/Customers/Views/BetList/AbnormalBetTicket.cshtml", vm);
        }
    }
}
