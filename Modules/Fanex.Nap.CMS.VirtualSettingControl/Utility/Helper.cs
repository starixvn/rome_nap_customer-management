namespace Fanex.Nap.CMS.VirtualSettingControl.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VrAutoAdjustmentReport;

    public static class Helper
    {
        public static IEnumerable<string> GetDate(DateTime currentTime, int monthRange, string format)
        {
            return MapDate(currentTime.AddMonths(-monthRange + 1), monthRange, d => d.ToString(format));
        }

        public static IEnumerable<MarginViewModel> GetMarginViewModels(DateTime date, int range, IEnumerable<MarginModel> history)
        {
            var temp = history.ToDictionary(m => Tuple.Create(m.Month, m.Year));

            return MapDate(date.AddMonths(-range + 1), range, d => MarginViewModelMapper(temp, d));
        }

        private static MarginViewModel MarginViewModelMapper(IDictionary<Tuple<int, int>, MarginModel> dict, DateTime d)
        {
            var key = Tuple.Create(d.Month, d.Year);

            if (dict.ContainsKey(key))
            {
                var model = dict[key];
                return new MarginViewModel(model.Winloss, model.Turnover, model.BetCount);
            }

            return new MarginViewModel(0, 0, 0);
        }

        public static IEnumerable<T> MapDate<T>(DateTime initDate, int range, Func<DateTime, T> mapFunc)
        {
            for (int i = 0; i < range; i++)
            {
                yield return mapFunc(initDate.AddMonths(i));
            }
        }

        public static void IterDate(DateTime initDate, int range, Action<DateTime> action)
        {
            for (int i = 0; i < range; i++)
            {
                action(initDate.AddMonths(i));
            }
        }
    }
}