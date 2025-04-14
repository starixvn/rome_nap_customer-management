namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests.PeakHour;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using System.Collections.Generic;
    using System.Linq;

    internal interface IPeakHourService
    {
        IEnumerable<PeakHourModel> GetPeakHours();
        Result AddPeakHour(AddPeakHourRequest request, int userId, string iP);
        Result DeletePeakHour(int rangeId, int userId, string iP);
    }
    internal class PeakHourService : ServiceBase, IPeakHourService
    {
        public PeakHourService() : base()
        {
        }

        public Result AddPeakHour(AddPeakHourRequest request, int userId, string iP)
        {
            return DoAction(new AddPeakHourCriteria(request.WeekDay, request.StartRange, request.EndRange, userId, iP), Labels.AddPeakHourSuccessfully);
        }

        public Result DeletePeakHour(int rangeId, int userId, string iP)
        {
            return DoAction(new DeletePeakHourCriteria(rangeId, userId, iP), Labels.DeletePeakHourSuccessfully);
        }

        public IEnumerable<PeakHourModel> GetPeakHours()
        {
            return DynamicRepository.Fetch<PeakHourModel>(new GetPeakHourCriteria()).OrderBy(peak => peak.WeekDay).ThenBy(peak => peak.StartRange);
        }
    }
}
