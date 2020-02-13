namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccountLicenseeSite
{
    public class ReduceBLLicenseeSiteViewModel
    {
        private int? maxPerMatchValue;
        private int? maxBetValue;

        public string Usernames { get; set; }

        public string MaxPerMatch { get; set; }

        public string MaxBet { get; set; }

        public int MaxPerMatchValue
        {
            get
            {
                maxPerMatchValue = maxPerMatchValue ?? ParseNumber(MaxPerMatch);

                return maxPerMatchValue.Value;
            }
        }

        public int MaxBetValue
        {
            get
            {
                maxBetValue = maxBetValue ?? ParseNumber(MaxBet);

                return maxBetValue.Value;
            }
        }

        public bool IsInvalid()
            => string.IsNullOrWhiteSpace(Usernames)
            || MaxPerMatchValue < 0
            || MaxBetValue < 0;

        private static int ParseNumber(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return -1;
            }

            int value;
            return int.TryParse(text.Replace(",", string.Empty), out value) ? value : -1;
        }
    }
}
