namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.Basketball
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public class VirtualRateRepository : IVirtualRateRepository
    {
        public IEnumerable<RateInfo> GetRateInfo(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        var id = Convert.ToInt32(reader["ValueId"]);

                        if (id != 0)
                        {
                            yield return new RateInfo
                            {
                                Id = id,
                                Value = Convert.ToInt32(reader["DisplayValue"])
                            };
                        }
                    }
                }
            }
        }

        public Tuple<IEnumerable<VirtualRateDTO>, VirtualRateDetailsReport> GetVirtualRateDetailsReport(ICriteria criteria)
        {
            var report = new VirtualRateDetailsReport();
            var virtualRates = new List<VirtualRateDTO>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        report = GetCustomerInfo(reader, report);

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                virtualRates.Add(GetVirtualRateDTO(reader));
                            }
                        }
                    }
                }

                var outputParams = db.GetOuputParams();
                report.ResultCode = Convert.ToInt32(outputParams["errcode"]);
                report.ResultMessage = outputParams["errmsg"].ToString();
            }

            return new Tuple<IEnumerable<VirtualRateDTO>, VirtualRateDetailsReport>(virtualRates, report);
        }

        private static VirtualRateDetailsReport GetCustomerInfo(IDataReader reader, VirtualRateDetailsReport report)
        {
            report.CustId = Convert.ToInt32(reader["CustId"]);
            report.Username = reader["Username"].ToString();
            report.Currency = reader["Currency"].ToString();
            report.ActualRate = Convert.ToDecimal(reader["ActualRate"]);
            report.UplineCustId = Convert.ToInt32(reader["UplineCustId"]);
            report.UplineCustName = reader["UplineUsername"].ToString();
            report.ReportLevel = Convert.ToByte(reader["RoleId"]);

            return report;
        }

        private static VirtualRateDTO GetVirtualRateDTO(IDataReader reader)
            => new VirtualRateDTO
            {
                SportType = Converter.ToInt(reader["SportType"]),
                LeagueGroupId = Converter.ToInt(reader["LeagueGroupID"], -1),
                LeagueGroupName = Converter.ToString2(reader["LeagueGroup"]),
                BetTypeGroupId = Converter.ToInt(reader["BetTypeGroupId"], -1),
                BetTypeGroupName = Converter.ToString2(reader["BetTypeGroup"]),
                BetTypeId = Converter.ToInt(reader["BetTypeId"], -1),
                Level = Converter.ToInt(reader["LevelId"]),
                LiveId = Converter.ToInt(reader["ValueIdLive"]),
                LiveValue = Converter.ToDecimal(reader["ValueLive"], -1),
                NonliveId = Converter.ToInt(reader["ValueIdNonLive"]),
                NonliveValue = Converter.ToDecimal(reader["ValueNonLive"], -1),
                LiveAutoSuspend = Converter.ToBool(reader["IsLiveAutoSuspend"], true),
                NonliveAutoSuspend = Converter.ToBool(reader["IsNonLiveAutoSuspend"], true),
                DisableLiveAutoSuspend = Converter.ToInt(reader["IsDisableLiveAutoSuspend"]) == 1,
                DisableNonliveAutoSuspend = Converter.ToInt(reader["IsDisableNonLiveAutoSuspend"]) == 1,
                LiveAutoSuspendThreshold = Converter.ToDecimal(reader["VRAutoSuspendThresholdLive"], -1),
                NonliveAutoSuspendThreshold = Converter.ToDecimal(reader["VRAutoSuspendThresholdNonLive"], -1)
            };
    }
}
