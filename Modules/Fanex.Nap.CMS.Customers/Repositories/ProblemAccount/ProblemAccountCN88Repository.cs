namespace Fanex.Nap.CMS.Customers.Repositories.ProblemAccount
{
    using System;
    using System.Data;
    using Models.ProblemAccount;

    public class ProblemAccountCN88Repository : ProblemAccountRepositoryBase
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
                reader["informed1by"].ToString(),
                reader["informed2by"].ToString(),
                null,
                0);
    }
}
