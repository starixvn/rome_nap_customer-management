namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using System;
    using Infrastructure.Constants;

    [Serializable]
    public class SearchViewModel
    {
        private const int FromDate = -13;

        public SearchViewModel()
        {
            To = DateTime.Today;
            From = To.AddDays(FromDate);
            Level = RoleIdConstants.Member;
            Currency = IndexViewModel.CY;
            OrderBy = IndexViewModel.OrderBySMALine;
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public byte Level { get; set; }

        public byte Status { get; set; }

        public int Currency { get; set; }

        public byte IsReduced { get; set; }

        public string Reason { get; set; }

        public string Username { get; set; }

        public bool IsSearchDownlines { get; set; }

        public byte OrderBy { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
