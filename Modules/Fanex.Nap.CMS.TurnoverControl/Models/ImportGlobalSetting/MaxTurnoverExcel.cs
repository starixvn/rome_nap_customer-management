namespace Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting
{
    public class MaxTurnoverExcel
    {
        public MaxTurnoverExcel(int profileId, string maxTurnover)
        {
            ProfileId = profileId;
            MaxTurnover = maxTurnover;
        }

        public int ProfileId { get; }

        public string MaxTurnover { get; }
    }
}
