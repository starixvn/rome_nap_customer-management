namespace Fanex.Nap.CMS.BetRestriction.Services
{
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Nap.CMS.BetRestriction.Criterias.Management;
    using Fanex.Nap.CMS.BetRestriction.Models.Management;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.ViewModels.Management;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using System.Data;
    using System.Text;

    public class BetRestrictionManagementService : ServiceBase, IBetRestrictionManagementService
    {
        private readonly ICMSConverter _cMSConverter;

        public BetRestrictionManagementService() : this(null)
        { }

        public BetRestrictionManagementService(ICMSConverter cMSConverter)
        {
            _cMSConverter = cMSConverter ?? new CMSConverterImplement();
        }

        public BetRestrictionSettingViewModel GetAllSettings(GetAllSettingCriteria criteria)
        {
            var result = new BetRestrictionSettingViewModel();
            var settings = Repository.Fetch<BetRestrictionSettingModel>(criteria);
            settings.ForEach(setting =>
            {
                switch (setting.RestrictionLevel)
                {
                    case "BetRestriction_Customer":
                        result.AccountSession.Add(setting);
                        break;
                    case "BetRestriction_Site":
                        result.SiteSession.Add(setting);
                        break;
                    case "BetRestriction_Currency":
                        result.CurrencySession.Add(setting);
                        break;
                    default:
                        break;
                }
            });

            return result;
        }

        public string ParseEditSettingsXML(EditSettingsRequest request)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<R>");
            foreach (var item in request.EditSettings)
            {
                xml.Append($"<Setting SettingId=\"{item.SettingId}\" ");
                if (item.LiveBetAcceptStatus != null)
                {
                    xml.Append($" LiveBetAccept=\"{_cMSConverter.ToInt32(item.LiveBetAcceptStatus)}\" ");
                }
                if (item.SeizeBetTicketStatus != null)
                {
                    xml.Append($" SeizeBetTicket=\"{_cMSConverter.ToInt32(item.SeizeBetTicketStatus)}\" ");
                }
                if (item.BetLimitStatus != null)
                {
                    xml.Append($" BetLimit=\"{_cMSConverter.ToInt32(item.BetLimitStatus)}\" ");
                }
                xml.Append(" />");
            }

            xml.Append("</R>");
            return xml.ToString();
        }
    }
}
