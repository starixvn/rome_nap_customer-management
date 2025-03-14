namespace Fanex.NAP.CMS.BetLimitControl.Serivces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease;
    using Fanex.NAP.CMS.BetLimitControl.DataAccess;
    using Fanex.NAP.CMS.BetLimitControl.Models.BetLimitIncrease;
    using Fanex.NAP.CMS.BetLimitControl.Request.BetLimitIncrease;
    using Fanex.NAP.CMS.BetLimitControl.Resources;

    public interface IBetLimitIncreaseService
    {
        IEnumerable<BetLimitIncreaseModel> SearchBetLimitIncrease(SearchBetLimitIncreaseRequest request);
        Result AddBetLimitIncrease(AddBetLimitIncreaseRequest request, string ip, int userId);
        Result UpdateBetLimitIncrease(BetLimitIncreaseModel request, string ip, int userId);
        Result DeleteBetLimitIncrease(BetLimitIncreaseModel request, string ip, int userId);
    }

    public class BetLimitIncreaseService : IBetLimitIncreaseService
    {
        private readonly BetLimitIncreaseDataAccess dataAccess;

        private const string CustIdFormat = "{{\"CustID\":\"{0}\", \"siteid\":\"{1}\"}},";
        private const string AllSportsValue = "0";
        private const string AllBettypesValue = "0";

        public BetLimitIncreaseService() : this(new BetLimitIncreaseDataAccess()) { }

        public BetLimitIncreaseService(BetLimitIncreaseDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public Result AddBetLimitIncrease(AddBetLimitIncreaseRequest request, string ip, int userId)
        {
            var checkUsernameCriteria = new CheckBetLimitIncreaseCriteria
            {
                Username = request.Username,
                SportTypes = AllSportsValue,
                Bettypes = AllBettypesValue,
                Multiplier = request.Multiplier
            };
            var validateUsername = dataAccess.ValidateUsername(checkUsernameCriteria);

            if (!validateUsername.Item1.IsSuccess)
            {
                return validateUsername.Item1;
            }

            var addBetLimitCriteria = new AddBetLimitIncreaseCriteria
            {
                CustIDs = BuildCustId(validateUsername.Item2),
                SportTypes = AllSportsValue,
                Bettypes = AllBettypesValue,
                Multiplier = request.Multiplier,
                Note = request.Note,
                IP = ip,
                UserID = userId
            };

            return dataAccess.DoAction(addBetLimitCriteria, Labels.AddBetLimitIncreaseSuccessMsg);
        }

        public Result UpdateBetLimitIncrease(BetLimitIncreaseModel request, string ip, int userId)
        {
            var criteria = new UpdateBetLimitIncreaseCriteria
            {
                CustIDs = BuildCustId(new List<BetLimitIncreaseModel> { request }),
                SportTypes = AllSportsValue,
                Bettypes = AllBettypesValue,
                Multiplier = request.Multiplier,
                Note = request.Note,
                IP = ip,
                UserID = userId
            };

            return dataAccess.DoAction(criteria, Labels.EditBetLimitInceaseSuccessMsg);
        }

        public Result DeleteBetLimitIncrease(BetLimitIncreaseModel request, string ip, int userId)
        {
            var criteria = new DeleteBetLimitIncreaseCriteria
            {
                CustIDs = BuildCustId(new List<BetLimitIncreaseModel> { request }),
                SportTypes = AllSportsValue,
                Bettypes = AllBettypesValue,
                IP = ip,
                UserID = userId
            };

            return dataAccess.DoAction(criteria);
        }

        public IEnumerable<BetLimitIncreaseModel> SearchBetLimitIncrease(SearchBetLimitIncreaseRequest request)
        {
            var criteria = new SearchBetLimitIncreaseCriteria
            {
                UserName = request.UserName,
                Currency = request.Currency,
                SiteID = request.SiteID
            };

            return dataAccess.SearchBetLimitIncrease(criteria);
        }

        private static string BuildCustId(IEnumerable<BetLimitIncreaseModel> usernames)
        {
            var custIds = new StringBuilder();
            custIds.Append("[");
            foreach (var username in usernames)
            {
                custIds.AppendFormat(CustIdFormat, username.CustId, username.SiteId);
            }

            if(usernames.Any())
            {
                custIds.Remove(custIds.Length - 1, 1);
            }
            custIds.Append("]");

            return custIds.ToString();
        }
    }
}
