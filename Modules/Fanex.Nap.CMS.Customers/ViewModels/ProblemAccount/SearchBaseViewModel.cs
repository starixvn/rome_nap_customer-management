namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using System;

    [Serializable]
    public abstract class SearchBaseViewModel
    {
        private const int FromDate = -13;

        protected SearchBaseViewModel()
        {
            To = DateTime.Today;
            From = To.AddDays(FromDate);
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int Currency { get; set; }

        public byte IsReduced { get; set; }

        public string Reason { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
