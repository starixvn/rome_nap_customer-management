namespace Fanex.Nap.CMS.Customers.ViewModels.CreditPayout
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class CreditPaypoutViewModel
    {
        public CreditPaypoutViewModel(bool hasHostPermission)
        {
            HasHostPermission = hasHostPermission;
            Promotions = new List<SelectListItem> { new SelectListItem { Text = "Correct Score Lucky Draw", Value = "1", Selected = true } };
            Status = new List<SelectListItem>
            {
                new SelectListItem { Text = "All", Value = "All", Selected = true },
                new SelectListItem { Text = "Open", Value = "0" },
                new SelectListItem { Text = "Verified", Value = "1" },
                new SelectListItem { Text = "Completed", Value = "2" },
                new SelectListItem { Text = "Cancelled", Value = "3" }
            };

            StatusEdit = new List<SelectListItem>
            {
                new SelectListItem { Text = "Please select", Value = "PleaseSelect" },
                new SelectListItem { Text = "Open", Value = "0" },
                new SelectListItem { Text = "Verified", Value = "1" },
                new SelectListItem { Text = "Completed", Value = "2" },
                new SelectListItem { Text = "Cancelled", Value = "3" }
            };
            CurrentDate = DateTime.Now.ToString("MM/dd/yyyy");
        }
        public bool HasHostPermission { get; }
        public IEnumerable<SelectListItem> Promotions { get; }

        public IEnumerable<SelectListItem> Status { get; }

        public IEnumerable<SelectListItem> StatusEdit { get; }

        public string CurrentDate { get; }
    }
}
