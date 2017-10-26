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
        public ImportGlobalSettingModel ValidateGlobalSettingExcels(ICriteria criteria)
        {
            IList<GlobalSettingItem> validSettings = new List<GlobalSettingItem>();
            IList<ErrorGlobalSettingItem> errorSettings = new List<ErrorGlobalSettingItem>();

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
                            errorSettings.Add(GetErrorGlobalSettingItem(reader));
                        }
                    }
                }
            }

            return new ImportGlobalSettingModel(validSettings, errorSettings);
        }

        public Result ImportGlobalSettings(ICriteria criteria)
        {
            Result result = new Result();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                result.Code = Convert.ToInt32(outputParams["errcode"]);
                result.Message = outputParams["errmsg"].ToString();
            }

            return result;
        }

        private static GlobalSettingItem GetGlobalSettingItem(IDataReader reader)
        {
            return new GlobalSettingItem
            {
                UserName = reader["username"].ToString(),
                Level = reader["level"].ToString(),
                Currency = reader["currency"].ToString(),
                MaxTurnover = Convert.ToDecimal(reader["maxturnover"])
            };
        }

        private static ErrorGlobalSettingItem GetErrorGlobalSettingItem(IDataReader reader)
        {
            return new ErrorGlobalSettingItem
            {
                UserName = reader["username"].ToString(),
                Level = reader["isgroupheadline"].ToString(),
                MaxTurnoverText = reader["maxturnover"].ToString(),
                Reason = reader["reason"].ToString()
            };
        }
    }
}
