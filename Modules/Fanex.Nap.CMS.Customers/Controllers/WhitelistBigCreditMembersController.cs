using System.Net;
using System.Web.Mvc;
using Fanex.Nap.CMS.Customers.Services.WhitelistBigCreditMembers;
using Fanex.Nap.CMS.Infrastructure;
using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Resources;
using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Attributes;

namespace Fanex.Nap.CMS.Customers.Controllers
{
    public class WhitelistBigCreditMembersController : BaseController
    {
        private readonly IWhitelistBigCreditMembersService service;

        public WhitelistBigCreditMembersController()
            : this(new WhitelistBigCreditMembersService())
        {
        }

        internal WhitelistBigCreditMembersController(IWhitelistBigCreditMembersService service)
        {
            this.service = service;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.WhitelistBigCreditMembers, Policy = Constant.Policy.Read)]
        public ActionResult Index()
            => View(HasRight(CMSFunctionKeys.WhitelistBigCreditMembers, Constant.Policy.Host));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.WhitelistBigCreditMembers, Policy = Constant.Policy.Host)]
        public ActionResult AddMembers(string usernames)
        {
            if (string.IsNullOrWhiteSpace(usernames))
            {
                return JsonData(
                    new[] { new Result((int)HttpStatusCode.BadRequest, Labels.BadRequest) },
                    JsonRequestBehavior.DenyGet);
            }

            return JsonData(
                service.AddMembers(usernames, CurrentSession.UserId, CurrentContext.IP),
                JsonRequestBehavior.DenyGet);
        }
    }
}
