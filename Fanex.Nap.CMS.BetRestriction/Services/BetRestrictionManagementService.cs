namespace Fanex.Nap.CMS.BetRestriction.Services
{
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
            using (var objDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria);
                while (reader.Read())
                {
                    var level = reader["RestrictionLevel"].ToString();
                    switch (level)
                    {
                        case "BetRestriction_Customer":
                            result.AccountSession.Add(MappingSP(reader));
                            break;
                        case "BetRestriction_Site":
                            result.SiteSession.Add(MappingSP(reader));
                            break;
                        case "BetRestriction_Currency":
                            result.CurrencySession.Add(MappingSP(reader));
                            break;
                        default:
                            break;
                    }
                }
            }

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

        private BetRestrictionSettingModel MappingSP(IDataReader reader)
        {
            return new BetRestrictionSettingModel
            {
                SettingId = _cMSConverter.ToInt32(reader["SettingID"]),
                SportType = _cMSConverter.ToInt32(reader["SportType"]),
                IsLicensee = _cMSConverter.ToBoolean(reader["IsLicensee"]),
                BetLimitStatus = _cMSConverter.ToBoolean(reader["BetLimitStatus"]),
                LiveBetAcceptStatus = _cMSConverter.ToBoolean(reader["LiveBetAcceptStatus"]),
                SeizeBetTicketStatus = _cMSConverter.ToBoolean(reader["SeizeBetTicketStatus"])
            };
        }
    }
}
