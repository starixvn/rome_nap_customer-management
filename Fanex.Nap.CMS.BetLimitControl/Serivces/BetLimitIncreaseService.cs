namespace Fanex.NAP.CMS.BetLimitControl.Serivces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease;
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
        private readonly IDynamicRepository dynamicRepository;

        private const string CustIdFormat = "{{\"CustID\":\"{0}\", \"siteid\":\"{1}\"}},";
        private const string AllSportsValue = "0";
        private const string AllBettypesValue = "0";

        public BetLimitIncreaseService() : this(new DynamicRepository()) { }

        public BetLimitIncreaseService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
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
            var validateUsernames = dynamicRepository.Fetch<BetLimitIncreaseModel>(checkUsernameCriteria);

            if (checkUsernameCriteria.ErrCode != 0)
            {
                return new Result(checkUsernameCriteria.ErrCode, checkUsernameCriteria.ErrMsg);
            }

            var addBetLimitCriteria = new AddBetLimitIncreaseCriteria
            {
                CustIDs = BuildCustId(validateUsernames),
                SportTypes = AllSportsValue,
                Bettypes = AllBettypesValue,
                Multiplier = request.Multiplier,
                Note = request.Note,
                IP = ip,
                UserID = userId
            };

            return DoAction(addBetLimitCriteria, Labels.AddBetLimitIncreaseSuccessMsg);
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

            return DoAction(criteria, Labels.EditBetLimitInceaseSuccessMsg);
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

            return DoAction(criteria);
        }

        public IEnumerable<BetLimitIncreaseModel> SearchBetLimitIncrease(SearchBetLimitIncreaseRequest request)
        {
            var criteria = new SearchBetLimitIncreaseCriteria
            {
                UserName = request.UserName,
                Currency = request.Currency,
                SiteID = request.SiteID
            };

            return dynamicRepository.Fetch<BetLimitIncreaseModel>(criteria);
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

        private Result DoAction(CommandBaseCriteria command, string successfullyMessage = null)
        {
            dynamicRepository.Execute(command);
            var message = command.ErrCode == 0
                    ? successfullyMessage ?? command.ErrMsg
                    : command.ErrMsg;

            return new Result { Code = command.ErrCode, Message = message };
        }
    }
}
