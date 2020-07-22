namespace Fanex.Nap.CMS.Customers.Repositories.ProblemAccountCreditSite
{
    using System;
    using System.Data;
    using Models.ProblemAccount;
    using ProblemAccount;

    public class ProblemAccountCreditSiteRepository : ProblemAccountRepositoryBase
    {
        protected override ProblemAccountItem ToProblemAccountItem(IDataReader reader)
            => new ProblemAccountItem(
                Convert.ToInt32(reader["custid"]),
                reader["username"].ToString(),
                reader["headline"].ToString(),
                reader["super"].ToString(),
                reader["master"].ToString(),
                reader["agent"].ToString(),
                Convert.ToBoolean(reader["isproblemaccsuper"]),
                Convert.ToBoolean(reader["isproblemaccmaster"]),
                Convert.ToBoolean(reader["isproblemaccagent"]),
                Convert.ToBoolean(reader["isproblemacc"]),
                ToNullableDecimal(reader["supermpm"]),
                ToNullableDecimal(reader["supermaxbet"]),
                ToNullableDecimal(reader["mastermpm"]),
                ToNullableDecimal(reader["mastermaxbet"]),
                ToNullableDecimal(reader["agentmpm"]),
                ToNullableDecimal(reader["agentmaxbet"]),
                ToNullableDecimal(reader["mpm"]),
                ToNullableDecimal(reader["maxbet"]),
                Convert.ToByte(reader["custstatus"]),
                Convert.ToDateTime(reader["markdate"]),
                reader["comment"].ToString(),
                ToNullableInt32(reader["abnormalbetnumber"]),
                null,
                null,
                reader["currency"].ToString(),
                Convert.ToByte(reader["actionid"]),
                reader["voidtime"].ToString(),
                Convert.ToDecimal(reader["limitamount"]));
    }
}
