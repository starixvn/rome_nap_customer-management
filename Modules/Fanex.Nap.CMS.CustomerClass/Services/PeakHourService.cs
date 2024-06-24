namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests.PeakHour;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.Framework.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal interface IPeakHourService
    {
        IEnumerable<PeakHourModel> GetPeakHours();
        Result AddPeakHour(AddPeakHourRequest request, int userId, string iP);
        Result DeletePeakHour(int rangeId, int userId, string iP);
    }
    internal class PeakHourService : IPeakHourService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IBaseRepository baseRepository;

        public PeakHourService() : this(new DynamicRepository(), new BaseRepository())
        {
        }

        public PeakHourService(IDynamicRepository dynamicRepository, IBaseRepository baseRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.baseRepository = baseRepository;
        }

        public Result AddPeakHour(AddPeakHourRequest request, int userId, string iP)
        {
            return baseRepository.DoAction(new AddPeakHourCriteria(request.WeekDay, request.StartRange, request.EndRange, userId, iP), Labels.AddPeakHourSuccessfully);
        }

        public Result DeletePeakHour(int rangeId, int userId, string iP)
        {
            return baseRepository.DoAction(new DeletePeakHourCriteria(rangeId, userId, iP), Labels.DeletePeakHourSuccessfully);
        }

        public IEnumerable<PeakHourModel> GetPeakHours()
        {
            return dynamicRepository.Fetch<PeakHourModel>(new GetPeakHourCriteria()).OrderBy(peak => peak.WeekDay).ThenBy(peak => peak.StartRange);
        }
    }
}
