using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageChatbots.AppStart
{
    public class MangeChatbotsAreaRegistration : AreaRegistration
    {
        public override string AreaName => "MangeChatbots";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ChatbotsDefault",
                "Chatbots/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Nap.CMS.ManageChatbots.Controllers" });

        }
    }
}
