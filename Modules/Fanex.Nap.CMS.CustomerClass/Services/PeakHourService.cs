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
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    internal interface IPeakHourService
    {
        SearchPeakHourResponse GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes, IEnumerable<LeagueGroup> leagueGroups);
        IEnumerable<PeakHourModel> GetPeakHours();
        Result AddPeakHour(AddEditPeakHourRequest request, int userId, string iP);
        Result UpdatePeakHour(AddEditPeakHourRequest request, int userId, string iP);
        Result DeletePeakHour(int rangeId, int userId, string iP);
    }
    internal class PeakHourService : ServiceBase, IPeakHourService
    {
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

        public PeakHourService() : base()
        {
        }

        public Result AddPeakHour(AddEditPeakHourRequest request, int userId, string iP)
        {
            return DoAction(new AddPeakHourCriteria(request.WeekDay, request.StartRange, request.EndRange, userId, iP), Labels.AddPeakHourSuccessfully);
        }

        public Result UpdatePeakHour(AddEditPeakHourRequest request, int userId, string iP)
        {
            return DoAction(new EditPeakHourCriteria(request.RangeId, request.WeekDay, request.StartRange, request.EndRange, userId, iP), Labels.EditPeakHourSuccessfully);
        }

        public Result DeletePeakHour(int rangeId, int userId, string iP)
        {
            return DoAction(new DeletePeakHourCriteria(rangeId, userId, iP), Labels.DeletePeakHourSuccessfully);
        }

        public IEnumerable<PeakHourModel> GetPeakHours()
        {
            return DynamicRepository.Fetch<PeakHourModel>(new GetPeakHourCriteria()).OrderBy(peak => peak.WeekDay).ThenBy(peak => peak.StartRange);
        }

        public SearchPeakHourResponse GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes, IEnumerable<LeagueGroup> leagueGroups)
        {
            var hasSpread = float.TryParse(request.Spread, out var spread);
            var criteria = new GetSpreadRuleCriteria
            {
                CustomerClasses = request.CustomerClass,
                Sites = request.Site,
                Currencies = request.Currency,
                LeagueCheck = request.LeagueGroups,
                LeagueGroups = BuildLeageGroupXML(leagueGroups),
                Spread = hasSpread ? spread : (float?)null,
                SportTypes = string.Join(",", sportTypes.Select(sp => sp.Id)),
                Type = SpreadType

            };
            var rules = DynamicRepository.Fetch<SpreadRule>(criteria);
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

        private string BuildLeageGroupXML(IEnumerable<LeagueGroup> leagueGroups)
        {
            var xmlBuilder = new StringBuilder();

            foreach (var lg in leagueGroups)
            {
                xmlBuilder
                .Append("<r GroupID=\"").Append(lg.Id).Append("\"")
                .Append(" GroupName=\"").Append(WebUtility.HtmlEncode(lg.Name)).Append("\"")
                .Append(" SportType=\"").Append(lg.SportType).Append("\"/>");
            }

            return $"<ROOT>{xmlBuilder.ToString()}</ROOT>";
        }
    }
}
