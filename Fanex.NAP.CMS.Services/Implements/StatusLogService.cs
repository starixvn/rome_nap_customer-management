namespace Fanex.NAP.CMS.Services.Implements
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CMS.Criteria.StatusLog;
    using Interfaces;
    using Models.StatusLog;
    using System.Xml;
    using Fanex.Common.Extensions;
    using Fanex.Common;

    public class StatusLogService : CMSBaseService, IStatusLogService
    {
        private readonly IDynamicRepository dynamicRepository;

        public StatusLogService() : this(new DynamicRepository())
        {
        }

        public StatusLogService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public StatusLogReport Logs(LogFilter filter)
        {
            var result = new StatusLogReport();

            var criteria = new LogCriteria
            {
                Code = filter.Code,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };

            var logs = dynamicRepository.Fetch<StatusLogItem>(criteria);
            logs.ForEach(log =>
            {
                log.NewCollStatus = GetCollStatus(trackingValue: log.TrackingValue);
                log.OldCollStatus = GetCollStatus(log.TrackingValue2);
            });

            result.Logs = logs;
            result.TotalRows = criteria.TotalRows;
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
