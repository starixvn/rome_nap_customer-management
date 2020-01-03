using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.ImportGlobalSetting
{
    public class ImportGlobalSettingRepository : IImportGlobalSettingRepository
    {
        public Tuple<IEnumerable<GlobalSettingItem>, IEnumerable<ErrorGlobalSettingExcel>> ValidateGlobalSettingExcels(ICriteria criteria)
        {
            IList<GlobalSettingItem> validSettings = new List<GlobalSettingItem>();
            IList<ErrorGlobalSettingExcel> errorSettings = new List<ErrorGlobalSettingExcel>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        validSettings.Add(GetGlobalSettingItem(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            errorSettings.Add(GetErrorGlobalSettingExcel(reader));
                        }
                    }
                }
            }

            return new Tuple<IEnumerable<GlobalSettingItem>, IEnumerable<ErrorGlobalSettingExcel>>(validSettings, errorSettings);
        }

        private static GlobalSettingItem GetGlobalSettingItem(IDataReader reader)
            => new GlobalSettingItem(
                reader["username"].ToString(),
                reader["level"].ToString(),
                reader["currency"].ToString(),
                Convert.ToInt32(reader["profileid"]),
                reader["profilename"].ToString(),
                Convert.ToDecimal(reader["maxturnover"]));

        private static ErrorGlobalSettingExcel GetErrorGlobalSettingExcel(IDataReader reader)
            => new ErrorGlobalSettingExcel(
                Convert.ToInt32(reader["rowid"]),
                reader["username"].ToString(),
                reader["field"].ToString(),
                reader["value"].ToString(),
                reader["reason"].ToString());

        public Result ImportGlobalSettings(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    outputParams["errmsg"].ToString());
            }
        }
    }
}
