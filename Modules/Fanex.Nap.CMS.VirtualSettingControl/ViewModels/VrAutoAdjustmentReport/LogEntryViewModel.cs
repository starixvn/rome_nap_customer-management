namespace Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VrAutoAdjustmentReport;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;

    public class LogEntryViewModel
    {
        private int _custId;

        private string _username;

        private string _updatedDate;

        private int _oldRate;

        private int _newRate;

        private IEnumerable<MarginViewModel> _marginViewModels;

        private MarginViewModel _avgPerformance;

        public LogEntryViewModel(VrAutoAdjustmentLogEntry log, IEnumerable<MarginModel> history)
        {
            MapLogData(log);

            MapHistoryData(history);
        }

        public int CustId
        {
            get { return _custId; }
        }

        public string Username
        {
            get { return _username; }
        }

        public string UpdatedDate
        {
            get { return _updatedDate; }
        }

        public int OldRate
        {
            get { return _oldRate; }
        }

        public int NewRate
        {
            get { return _newRate; }
        }

        public IEnumerable<MarginViewModel> MarginViewModels
        {
            get { return _marginViewModels; }
        }

        public MarginViewModel AvgPerformance
        {
            get { return _avgPerformance; }
        }

        private void MapLogData(VrAutoAdjustmentLogEntry log)
        {
            _custId = log.CustId;
            _username = log.Username;
            _updatedDate = log.UpdatedDate.ToString("MM/dd/yyyy");
            _oldRate = log.OldRate;
            _newRate = log.NewRate;
        }

        private void MapHistoryData(IEnumerable<MarginModel> history)
        {
            _marginViewModels = Helper.GetMarginViewModels(DateTime.Today, 6, history);

            _avgPerformance =
                _marginViewModels.Aggregate(
                    new Tuple<decimal, decimal>(0, 0),
                    (t, m) => new Tuple<decimal, decimal>(t.Item1 + m.Winloss, t.Item2 + m.Turnover),
                    t => new MarginViewModel(t.Item1, t.Item2));
        }
    }
}