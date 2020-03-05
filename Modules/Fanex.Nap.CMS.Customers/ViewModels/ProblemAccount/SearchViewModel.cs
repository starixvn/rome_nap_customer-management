namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using System;
    using Constants;
    using Infrastructure.Constants;

    [Serializable]
    public class SearchViewModel : SearchBaseViewModel
    {
        public SearchViewModel()
        {
            Level = RoleIdConstants.Member;
            OrderBy = OrderByConst.OrderBySMALine;
        }

        public byte Level { get; set; }

        public byte Status { get; set; }

        public byte IsDirectlyMarked { get; set; }

        public string Username { get; set; }

        public bool IsSearchDownlines { get; set; }

        public byte OrderBy { get; set; }
    }
}
