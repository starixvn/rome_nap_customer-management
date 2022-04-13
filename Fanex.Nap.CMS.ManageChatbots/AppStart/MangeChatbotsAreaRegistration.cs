namespace Fanex.Nap.CMS.ManageChatbots.AppStart
{
    using System.Web.Mvc;
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
