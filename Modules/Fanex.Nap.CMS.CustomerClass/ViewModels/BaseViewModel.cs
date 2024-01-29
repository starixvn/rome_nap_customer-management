namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class BaseViewModel
    {
        protected BaseViewModel(bool hasHostPermission)
            => HasHostPermission = hasHostPermission;

        public bool HasHostPermission { get; }

        protected static SelectListItem ToSelectListItem(short value)
        {
            var valueString = value.ToString();

            return new SelectListItem { Text = valueString, Value = valueString };
        }

        protected static SelectListItem ToSelectListItem(IdNameModel model)
            => new SelectListItem { Text = model.Name, Value = model.Id.ToString() };

        protected static SelectListItem ToSelectListItem(KeyValuePair<short, string> kvp)
            => new SelectListItem { Text = kvp.Value, Value = kvp.Key.ToString() };

        protected static SelectListItem ToSelectListItem(KeyValuePair<byte, string> kvp)
            => new SelectListItem { Text = kvp.Value, Value = kvp.Key.ToString() };
    }
}
