namespace Fanex.NAP.CMS.Models.LossMonitor
{
    using Core.Resources;
    using Core.Web;

    public class SMALossInfo
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public byte RoleId { get; set; }

        public string Currency { get; set; }

        public int GroupHeadlineId { get; set; }

        public decimal? YesterdayBalance { get; set; }

        public decimal? TotalBalance { get; set; }

        public decimal? MaxLoss { get; set; }

        public byte LevelId { get; set; }

        public int? Movement { get; set; }

        public decimal? PercentageLoss { get; set; }

        public bool NotifyLetsTalk { get; set; }

        public string RoleName
        {
            get
            {
                return GroupHeadlineId > 0 ? ViewLang.Headline : CacheRef.GetLevel(RoleId);
            }
        }
    }
}