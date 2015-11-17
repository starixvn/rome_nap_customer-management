namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting;

    public class AddPlayerWinLimitCriteria : CriteriaBase
    {
        public IList<AddPlayerWinLimitViewModel> PlayerSettings { get; set; }

        public int Player { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSettingIns";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            {
                { "CustSettingXML", BuildCustomerSettingsXml() },
                { "UserID",  AppSession.CurrentUser.Id },
                { "IP",  IPHelper.ClientIP() },
                { "ErrCode",  ErrCode },
                { "ErrMsg",  ErrMsg }
            };
        }

        private string BuildCustomerSettingsXml()
        {
            string format = @"<row CustID=""{0}"" ProductID=""{1}"" MaxWinLimit=""{2}""/>";

            StringBuilder builder = new StringBuilder();
            builder.Append("<custsetting>");
            foreach (var item in PlayerSettings)
            {
                if (item.PlayerWinLimit <= item.GlobalWinLimit)
                {
                    builder.AppendFormat(format, Player, item.Game, item.PlayerWinLimit);
                }
            }

            builder.Append("</custsetting>");
            return builder.ToString();
        }
    }
}
