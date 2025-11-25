namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.TurnoverControl.Constants;
    using Fanex.Nap.CMS.TurnoverControl.Services.LeagueGroupSetting;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class TurnoverLeagueGroupController : BaseController
    {
        private readonly ITurnoverLeagueGroupService turnoverLeagueGroupService;
        public TurnoverLeagueGroupController()
            : this(null)
        {
        }
        public TurnoverLeagueGroupController(ITurnoverLeagueGroupService turnoverLeagueGroupService)
        {
            this.turnoverLeagueGroupService = turnoverLeagueGroupService ?? new TurnoverLeagueGroupService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverControlLeagueGroupSoccer, Policy = Constant.Policy.Read)]
        public ActionResult Index(short? t)
        {
            ViewData["HostPermission"] = HasRight(CMSFunctionKeys.TurnoverControlLeagueGroupSoccer, Constant.Policy.Host);
            var viewModel = turnoverLeagueGroupService.GetLeagueGroupViewModel(SportTypeConst.Soccer);

            return View("~/Areas/TurnoverControl/Views/LeagueGroup/Index.cshtml", viewModel);
        }
    }
}
