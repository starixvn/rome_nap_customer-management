namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    public class ReduceBLViewModel : ActionViewModel
    {
        private int? maxPerEventValue;
        private int? maxPerBetValue;

        public string MaxPerEvent { get; set; }

        public string MaxPerBet { get; set; }

        public int MaxPerEventValue
        {
            get
            {
                maxPerEventValue = maxPerEventValue ?? ParseNumber(MaxPerEvent);

                return maxPerEventValue.Value;
            }
        }

        public int MaxPerBetValue
        {
            get
            {
                maxPerBetValue = maxPerBetValue ?? ParseNumber(MaxPerBet);

                return maxPerBetValue.Value;
            }
        }

        public override bool IsInvalid()
            => base.IsInvalid()
            || MaxPerEventValue <= 0
            || MaxPerBetValue <= 0;

        private static int ParseNumber(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            int value;
            int.TryParse(text.Replace(",", string.Empty), out value);

            return value;
        }
    }
}
