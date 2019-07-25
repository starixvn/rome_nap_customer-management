namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public static class VirtualRateDetailsData
    {
        public static IEnumerable<LogDTO> GetLogs(GetVirtualRateLogsCriteria criteria)
        {
            var result = new List<LogDTO>
            {
                new LogDTO {
                    Username ="Agent01",
                    Editor="User2",
                    UpdatedDate = new System.DateTime(2019, 3, 17, 23, 59, 0),
                    NewValue = Log1(),
                    OldValue = OldLog1()
                },
                new LogDTO {
                    Username ="Member01",
                    Editor="User1",
                    UpdatedDate = new System.DateTime(2019, 1, 15, 23, 59, 0),
                    NewValue = Log2(),
                    OldValue = OldLog2()
                }
            };

            return result;
        }

        private static string Log1()
        {
            var result = new List<string>
            {
                "<Root ExchangeRate=\"1.4000\">",
                "<Row LevelID=\"1\" S=\"43\" L=\"\" LN=\"\" BG=\"\" BGN=\"\" BT=\"\" BTN=\"\" Live=\"100\" NonLive=\"100\" IsFollow=\"true\" Layer=\"Global\" />",
                "<Row LevelID=\"2\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"\" BGN=\"\" BT=\"\" BTN=\"\" Live=\"80\" NonLive=\"80\" IsFollow=\"true\" Layer=\"Super\" />",
                "<Row LevelID=\"3\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"\" BTN=\"\" Live=\"33\" NonLive=\"33\" IsFollow=\"true\" Layer=\"Master\" />",
                "<Row LevelID=\"4\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"1\" BTN=\"Map X Clutches Handicap\" Live=\"23\" NonLive=\"23\" IsFollow=\"true\" Layer=\"Master\" />",
                "<Row LevelID=\"5\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"2\" BTN=\"Map X Round Y Total Kills Handicap\" Live=\"30\" NonLive=\"14\" IsFollow=\"false\" Layer=\"\" />",
                "</Root>"
            };

            return string.Concat(result);
        }

        private static string OldLog1()
        {
            var result = new List<string>
            {
                "<Root ExchangeRate=\"1.4000\">",
                "<Row LevelID=\"1\" S=\"43\" L=\"\" LN=\"\" BG=\"\" BGN=\"\" BT=\"\" BTN=\"\" Live=\"100\" NonLive=\"100\" IsFollow=\"true\" Layer=\"Global\" />",
                "<Row LevelID=\"2\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"\" BGN=\"\" BT=\"\" BTN=\"\" Live=\"80\" NonLive=\"80\" IsFollow=\"true\" Layer=\"Super\" />",
                "<Row LevelID=\"3\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"\" BTN=\"\" Live=\"33\" NonLive=\"33\" IsFollow=\"true\" Layer=\"Master\" />",
                "<Row LevelID=\"4\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"1\" BTN=\"Map X Clutches Handicap\" Live=\"23\" NonLive=\"23\" IsFollow=\"true\" Layer=\"Master\" />",
                "<Row LevelID=\"5\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"2\" BTN=\"Map X Round Y Total Kills Handicap\" Live=\"14\" NonLive=\"14\" IsFollow=\"false\" Layer=\"\" />",
                "</Root>"
            };

            return string.Concat(result);
        }

        private static string Log2()
        {
            var result = new List<string>
            {
                "<Root ExchangeRate=\"1.2300\">",
                "<Row LevelID=\"1\" S=\"43\" L=\"\" LN=\"\" BG=\"\" BGN=\"\" BT=\"\" BTN=\"\" Live=\"100\" NonLive=\"100\" IsFollow=\"true\" Layer=\"Global\" />",
                "<Row LevelID=\"2\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"\" BGN=\"\" BT=\"\" BTN=\"\" Live=\"80\" NonLive=\"80\" IsFollow=\"true\" Layer=\"Super\" />",
                "<Row LevelID=\"3\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"\" BTN=\"\" Live=\"33\" NonLive=\"33\" IsFollow=\"true\" Layer=\"Master\" />",
                "<Row LevelID=\"4\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"1\" BTN=\"Map X Clutches Handicap\" Live=\"23\" NonLive=\"23\" IsFollow=\"true\" Layer=\"Master\" />",
                "<Row LevelID=\"5\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"2\" BTN=\"Map X Round Y Total Kills Handicap\" Live=\"14\" NonLive=\"14\" IsFollow=\"true\" Layer=\"Agent\" />",
                "<Row LevelID=\"6\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"3\" BTN=\"Map X Total Barracks Handicap\" Live=\"31\" NonLive=\"31\" IsFollow=\"false\" Layer=\"\" />",
                "<Row LevelID=\"7\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"2\" BGN=\"Moneyline\" BT=\"\" BTN=\"\" Live=\"10\" NonLive=\"10\" IsFollow=\"false\" Layer=\"\" />",
                "<Row LevelID=\"8\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"2\" BGN=\"Moneyline\" BT=\"1\" BTN=\"Map X Moneyline\" Live=\"5\" NonLive=\"5\" IsFollow=\"false\" Layer=\"\" />",
                "<Row LevelID=\"9\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"2\" BGN=\"Moneyline\" BT=\"2\" BTN=\"Map X Round Y Total Kills Moneyline\" Live=\"10\" NonLive=\"10\" IsFollow=\"false\" Layer=\"\" />",
                "</Root>"
            };

            return string.Concat(result);
        }

        private static string OldLog2()
        {
            var result = new List<string>
            {
                 "<Root ExchangeRate=\"1.2300\">",
                "<Row LevelID=\"1\" S=\"43\" L=\"\" LN=\"\" BG=\"\" BGN=\"\" BT=\"\" BTN=\"\" Live=\"100\" NonLive=\"100\" IsFollow=\"true\" Layer=\"Global\" />",
                "<Row LevelID=\"2\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"\" BGN=\"\" BT=\"\" BTN=\"\" Live=\"80\" NonLive=\"80\" IsFollow=\"true\" Layer=\"Super\" />",
                "<Row LevelID=\"3\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"\" BTN=\"\" Live=\"33\" NonLive=\"33\" IsFollow=\"true\" Layer=\"Master\" />",
                "<Row LevelID=\"4\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"1\" BTN=\"Map X Clutches Handicap\" Live=\"23\" NonLive=\"23\" IsFollow=\"true\" Layer=\"Master\" />",
                "<Row LevelID=\"5\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"2\" BTN=\"Map X Round Y Total Kills Handicap\" Live=\"14\" NonLive=\"14\" IsFollow=\"true\" Layer=\"Agent\" />",
                "<Row LevelID=\"6\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"1\" BGN=\"Handicap\" BT=\"3\" BTN=\"Map X Total Barracks Handicap\" Live=\"31\" NonLive=\"31\" IsFollow=\"false\" Layer=\"\" />",
                "<Row LevelID=\"7\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"2\" BGN=\"Moneyline\" BT=\"\" BTN=\"\" Live=\"10\" NonLive=\"10\" IsFollow=\"false\" Layer=\"\" />",
                "<Row LevelID=\"8\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"2\" BGN=\"Moneyline\" BT=\"1\" BTN=\"Map X Moneyline\" Live=\"5\" NonLive=\"5\" IsFollow=\"false\" Layer=\"\" />",
                "<Row LevelID=\"9\" S=\"43\" L=\"1\" LN=\"CS:GO\" BG=\"2\" BGN=\"Moneyline\" BT=\"2\" BTN=\"Map X Round Y Total Kills Moneyline\" Live=\"15\" NonLive=\"15\" IsFollow=\"false\" Layer=\"\" />",
                "</Root>"
            };

            return string.Concat(result);
        }
    }
}
