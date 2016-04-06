namespace Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Services.VrAutoAdjustmentReport;

    public class CustomerMarginHistory
    {
        private readonly double avgTurnover;

        private readonly double avgWinloss;

        private readonly double margin;

        private readonly IEnumerable<string> months;

        private readonly IEnumerable<double> turnovers, winlosses, margins;

        public CustomerMarginHistory(IEnumerable<Models.VrAutoAdjustmentReport.CustomerMarginHistory> logEntries, IMarginCalculatorService calculator, Func<int, string> monthGetter)
        {
            avgTurnover = calculator.CalculateTurnover(logEntries);
            avgWinloss = calculator.CalculateWinloss(logEntries);
            margin = calculator.CalculateMargin(avgTurnover, avgWinloss);
            months = logEntries.Select(l => monthGetter(l.Month));
            turnovers = logEntries.Select(l => l.Turnover);
            winlosses = logEntries.Select(l => l.Winloss);
            margins = logEntries.Select(l => l.Margin);
        }

        public double AvgTurnover
        {
            get { return avgTurnover; }
        }

        public double AvgWinloss
        {
            get { return avgWinloss; }
        }

        public double Margin
        {
            get { return margin; }
        }

        public IEnumerable<string> Months
        {
            get { return months; }
        }

        public IEnumerable<double> Turnovers
        {
            get { return turnovers; }
        }

        public IEnumerable<double> Winlosses
        {
            get { return winlosses; }
        }

        public IEnumerable<double> Margins
        {
            get { return margins; }
        }
    }
}