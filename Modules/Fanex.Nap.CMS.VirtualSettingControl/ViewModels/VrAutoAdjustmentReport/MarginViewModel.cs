namespace Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VrAutoAdjustmentReport
{
    public class MarginViewModel
    {
        private readonly decimal _winloss;

        private readonly decimal _turnover;

        public MarginViewModel(decimal winloss, decimal turnover)
        {
            _winloss = winloss;
            _turnover = turnover;
        }

        public decimal Winloss
        {
            get { return _winloss; }
        }

        public decimal Turnover
        {
            get { return _turnover; }
        }

        public decimal Margin
        {
            get { return _winloss == 0 ? 0 : _winloss / _turnover; }
        }
    }
}