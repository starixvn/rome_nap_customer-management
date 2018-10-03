namespace Fanex.NAP.CMS.Repositories.Implements
{
    using System.Collections.Generic;
    using System.Data;
    using System.Xml;
    using Common;
    using Criteria.StatusLog;
    using Data;
    using Interfaces;
    using Models.StatusLog;

    public class StatusLogRepository : IStatusLogRepository
    {
        public StatusLogReport Logs(LogFilter filter)
        {
            var result = new StatusLogReport();

            var criteria = new LogCriteria
            {
                Code = filter.Code,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };

            var logs = new List<StatusLogItem>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var dataset = db.ExecuteDataSet(criteria.GetSpParams()))
                {
                    if (dataset.Tables.Count > 0)
                    {
                        var list = dataset.Tables[0];

                        foreach (DataRow row in list.Rows)
                        {
                            logs.Add(new StatusLogItem
                            {
                                LogID = Converter.To<long>(row["LogID"]),
                                LogDate = Converter.ToDateTime(row["LogDate"]),
                                EditorId = Converter.ToInt(row["EditorId"]),
                                EditorName = Converter.ToString2(row["EditorName"]),
                                Action = Converter.ToString2(row["ActionNameEN"]),
                                SPName = Converter.ToString2(row["SPName"]),
                                NewCollStatus = GetCollStatus(Converter.ToString2(row["TrackingValue"])),
                                OldCollStatus = GetCollStatus(Converter.ToString2(row["TrackingValue2"]))
                            });
                        }
                    }
                }

                var param = db.GetOuputParams();
                result.TotalRows = Converter.ToInt(param["totalrows"]);
            }

            result.Logs = logs;
            return result;
        }

        private static string GetCollStatus(string trackingValue)
        {
            var doc = new XmlDocument();
            doc.LoadXml(trackingValue);

            return Converter.ToString2(doc.FirstChild.Attributes["Coll_Status"].Value);
        }
    }
}
