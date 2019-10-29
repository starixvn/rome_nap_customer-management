namespace Fanex.Nap.CMS.Customers.ViewModels
{
    using System.Web.Mvc;

    public class BaseViewModel
    {
        protected static SelectListItem BuildSelectListItem(int value, string name, bool selected = false)
            => new SelectListItem { Value = value.ToString(), Text = name, Selected = selected };
    }
}
