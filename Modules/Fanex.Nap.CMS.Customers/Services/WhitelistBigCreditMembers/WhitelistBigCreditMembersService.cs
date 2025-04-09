using System.Collections.Generic;
using System.Linq;
using Fanex.Nap.CMS.Customers.Criteria.WhitelistBigCreditMembers;
using Fanex.Nap.CMS.Customers.Models.WhitelistBigCreditMembers;
using Fanex.Nap.CMS.Customers.Resources;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;

namespace Fanex.Nap.CMS.Customers.Services.WhitelistBigCreditMembers
{
    public class WhitelistBigCreditMembersService : ServiceBase, IWhitelistBigCreditMembersService
    {
        public IEnumerable<AddMemberResult> AddMembers(string usernames, int userId, string ip)
        {
            var results = new List<AddMemberResult>();

            var addMemberResults = DynamicRepository.Fetch<AddMemberResult>(new AddMemberCriteria(usernames, userId, ip));
            var groupResults = addMemberResults.GroupBy(r => new { r.Code, r.Message });

            foreach (var group in groupResults)
            {
                var joinUsernames = string.Join(", ", group.Select(r => r.Username));
                results.Add(new AddMemberResult(joinUsernames, group.Key.Code, group.Key.Message));
            }

            if (results.Count == 1 && results[0].IsSuccess)
            {
                return new[] { new AddMemberResult(string.Empty, 0, WhitelistBigCreditMembersResource.AddMembersSuccessfully) };
            }

            return results;
        }
    }
}
