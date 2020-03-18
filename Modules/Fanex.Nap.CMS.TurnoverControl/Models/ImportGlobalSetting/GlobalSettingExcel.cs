using System.Collections.Generic;

namespace Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting
{
    public class GlobalSettingExcel
    {
        public GlobalSettingExcel(
            string username,
            string headline,
            IEnumerable<MaxTurnoverExcel> maxTurnovers,
            int excelRow = 0)
        {
            Username = username;
            Headline = headline;
            MaxTurnovers = maxTurnovers;
            ExcelRow = excelRow;
        }

        public string Username { get; }

        public string Headline { get; }

        public IEnumerable<MaxTurnoverExcel> MaxTurnovers { get; }

        public int ExcelRow { get; }
    }
}
