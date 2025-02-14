using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Infrastructure.Criteria;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.ImportGlobalSetting
{
    public class ImportGlobalSettingRepository : IImportGlobalSettingRepository
    {
        public Tuple<IEnumerable<GlobalSettingItem>, IEnumerable<ErrorGlobalSettingExcel>> ValidateGlobalSettingExcels(CriteriaBase criteria)
        {
            IList<GlobalSettingItem> validSettings = new List<GlobalSettingItem>();
            IList<ErrorGlobalSettingExcel> errorSettings = new List<ErrorGlobalSettingExcel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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
                //Convert.ToBoolean(reader["islock"]),
                false,
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

        public Result ImportGlobalSettings(CommandBaseCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);

                return new Result(criteria.ErrCode, criteria.ErrMsg);
            }
        }
    }
}
