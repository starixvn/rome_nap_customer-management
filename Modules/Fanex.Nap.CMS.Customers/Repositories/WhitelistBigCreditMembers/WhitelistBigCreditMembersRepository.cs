using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Customers.Models.WhitelistBigCreditMembers;

namespace Fanex.Nap.CMS.Customers.Repositories.WhitelistBigCreditMembers
{
    public class WhitelistBigCreditMembersRepository : IWhitelistBigCreditMembersRepository
    {
        public IEnumerable<AddMemberResult> AddMembers(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return ToAddMemberResult(reader);
                    }
                }
            }
        }

        private static AddMemberResult ToAddMemberResult(IDataReader reader)
            => new AddMemberResult(
                reader["username"].ToString(),
                Convert.ToInt32(reader["errorcode"]),
                reader["errormsg"].ToString());
    }
}
