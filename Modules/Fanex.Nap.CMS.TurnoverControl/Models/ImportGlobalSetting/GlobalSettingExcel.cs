namespace Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting
{
    public sealed class GlobalSettingExcel
    {
        public GlobalSettingExcel()
        {
        }

        public GlobalSettingExcel(string userName, string headline, string maxTurnover)
        {
            UserName = userName;
            Headline = headline;
            MaxTurnover = maxTurnover;
        }

        public string UserName { get; set; }

        public string Headline { get; set; }

        public string MaxTurnover { get; set; }
    }
}
