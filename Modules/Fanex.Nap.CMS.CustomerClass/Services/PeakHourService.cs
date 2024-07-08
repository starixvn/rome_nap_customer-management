namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Requests.PeakHour;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.Framework.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal interface IPeakHourService
    {
        SearchPeakHourResponse GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes);
        IEnumerable<PeakHourModel> GetPeakHours();
        Result AddPeakHour(AddEditPeakHourRequest request, int userId, string iP);
        Result UpdatePeakHour(AddEditPeakHourRequest request, int userId, string iP);
        Result DeletePeakHour(int rangeId, int userId, string iP);
    }
    internal class PeakHourService : IPeakHourService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IBaseRepository baseRepository;
        private const byte SpreadType = 2;
        private const string PeakHourRangeFormat = ", {0} - {1}";
        private const string PeakHourTimeFormat = @"h\:mm";
        private const string PeakHourNoteFormat = "<p>Peak Hour Range:</p>{0}";
        private const string PeakHourByDateFormat = "<p>{0}: {1}</p>";

        protected static readonly IDictionary<int, string> DateNames = new Dictionary<int, string>
        {
            [2] = "Mon",
            [3] = "Tue",
            [4] = "Wed",
            [5] = "Thu",
            [6] = "Fri",
            [7] = "Sat",
            [1] = "Sun"
        };

        public PeakHourService() : this(new DynamicRepository(), new BaseRepository())
        {
        }

        public PeakHourService(IDynamicRepository dynamicRepository, IBaseRepository baseRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.baseRepository = baseRepository;
        }

        public Result AddPeakHour(AddEditPeakHourRequest request, int userId, string iP)
        {
            return baseRepository.DoAction(new AddPeakHourCriteria(request.WeekDay, request.StartRange, request.EndRange, userId, iP), Labels.AddPeakHourSuccessfully);
        }

        public Result UpdatePeakHour(AddEditPeakHourRequest request, int userId, string iP)
        {
            return baseRepository.DoAction(new EditPeakHourCriteria(request.RangeId, request.WeekDay, request.StartRange, request.EndRange, userId, iP), Labels.EditPeakHourSuccessfully);
        }

        public Result DeletePeakHour(int rangeId, int userId, string iP)
        {
            return baseRepository.DoAction(new DeletePeakHourCriteria(rangeId, userId, iP), Labels.DeletePeakHourSuccessfully);
        }

        public IEnumerable<PeakHourModel> GetPeakHours()
        {
            return dynamicRepository.Fetch<PeakHourModel>(new GetPeakHourCriteria()).OrderBy(peak => peak.WeekDay).ThenBy(peak => peak.StartRange);
        }

        public SearchPeakHourResponse GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes)
        {
            var rules = dynamicRepository.Fetch<SpreadRule>(new GetSpreadRuleCriteria(request, string.Join(",", sportTypes.Select(sp => sp.Id)), SpreadType));
            var peakNote = BuildPeakHourNote();

            return new SearchPeakHourResponse
            {
                SpreadRules = rules,
                PeakNoteInfo = peakNote
            };
        }

        private string BuildPeakHourNote()
        {
            var peakHourByDates = GetPeakHours().GroupBy(peak => peak.WeekDay);
            if (peakHourByDates.Any())
            {
                var range = new StringBuilder();
                foreach (var peakHourByDate in peakHourByDates)
                {
                    range.AppendFormat(PeakHourByDateFormat, DateNames[peakHourByDate.Key], BuildRangeHour(peakHourByDate.ToList()));
                }

                return string.Format(PeakHourNoteFormat, range);
            }

            return string.Empty;
        }

        private string BuildRangeHour(List<PeakHourModel> peakHours)
        {
            var mergePeakHours = new List<PeakHourModel>() { peakHours[0] };
            int currentSelectPeakHourIndex = 0;
            foreach (var peakHour in peakHours)
            {
                if(peakHours[currentSelectPeakHourIndex].EndRange >= peakHour.StartRange)
                {
                    peakHours[currentSelectPeakHourIndex].EndRange = peakHours[currentSelectPeakHourIndex].EndRange >= peakHour.EndRange ? peakHours[currentSelectPeakHourIndex].EndRange : peakHour.EndRange;
                } else
                {
                    mergePeakHours.Add(peakHour);
                    currentSelectPeakHourIndex++;
                }
            }

            var peakString = new StringBuilder();
            foreach (var peakHour in mergePeakHours)
            {
                peakString.AppendFormat(PeakHourRangeFormat, peakHour.StartRange.ToString(PeakHourTimeFormat), peakHour.EndRange.ToString(PeakHourTimeFormat));
            }

            return peakString.ToString().Remove(0, 2);
        }
    }
}
