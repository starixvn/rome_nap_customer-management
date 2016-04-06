namespace Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Services.VrAutoAdjustmentReport;

    public class CustomerMarginHistory
    {
        private readonly double _avgTurnover;

        private readonly double _avgWinloss;

        private readonly double _margin;

        private readonly IEnumerable<string> _months;

        private readonly IEnumerable<double> _turnovers, _winlosses, _margins;

        public CustomerMarginHistory(IEnumerable<Models.VrAutoAdjustmentReport.CustomerMarginHistory> logEntries, IMarginCalculatorService calculator, Func<int, string> monthGetter)
        {
            _avgTurnover = calculator.CalculateTurnover(logEntries);
            _avgWinloss = calculator.CalculateWinloss(logEntries);
            _margin = calculator.CalculateMargin(_avgTurnover, _avgWinloss);
            _months = logEntries.Select(l => monthGetter(l.Month));
            _turnovers = logEntries.Select(l => l.Turnover);
            _winlosses = logEntries.Select(l => l.Winloss);
            _margins = logEntries.Select(l => l.Margin);
        }

        public double AvgTurnover
        {
            get { return _avgTurnover; }
        }

        public double AvgWinloss
        {
            get { return _avgWinloss; }
        }

        public double Margin
        {
            get { return _margin; }
        }

        public IEnumerable<string> Months
        {
            get { return _months; }
        }

        public IEnumerable<double> Turnovers
        {
            get { return _turnovers; }
        }

        public IEnumerable<double> Winlosses
        {
            get { return _winlosses; }
        }

        public IEnumerable<double> Margins
        {
            get { return _margins; }
        }
    }
}