namespace Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting
{
    public class GameWinLimit
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal PlayerWinLimit { get; set; }

        public decimal GlobalWinLimit { get; set; }
    }
}
