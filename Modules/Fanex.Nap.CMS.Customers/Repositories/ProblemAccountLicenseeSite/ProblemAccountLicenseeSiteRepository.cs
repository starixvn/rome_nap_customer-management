namespace Fanex.Nap.CMS.Customers.Repositories.ProblemAccountLicenseeSite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models.ProblemAccount;
    using Models.ProblemAccountLicenseeSite;
    using ProblemAccount;

    public class ProblemAccountLicenseeSiteRepository : ProblemAccountRepositoryBase, IProblemAccountLicenseeSiteRepository
    {
        protected override ProblemAccountItem ToProblemAccountItem(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LicenseeAcknowledgement> GetLicenseeAcknowledgements(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return ToLicenseeAcknowledgement(reader);
                    }
                }
            }
        }

        private static LicenseeAcknowledgement ToLicenseeAcknowledgement(IDataReader reader)
            => new LicenseeAcknowledgement(
                Convert.ToInt32(reader["licenseeid"]),
                reader["licenseename"].ToString(),
                Convert.ToBoolean(reader["isactive"]),
                Convert.ToBoolean(reader["isacknowledgement"]));

        Tuple<IEnumerable<ProblemAccountLicenseeSiteItem>, IEnumerable<ProblemAccountReason>, int> IProblemAccountLicenseeSiteRepository.GetProblemAccounts(ICriteria criteria)
        {
            IList<ProblemAccountLicenseeSiteItem> problemAccounts = new List<ProblemAccountLicenseeSiteItem>();
            IList<ProblemAccountReason> reasons = new List<ProblemAccountReason>();
            int totalItems = 0;

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        problemAccounts.Add(ToProblemAccountLicenseeSiteItem(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            reasons.Add(ToProblemAccountReason(reader));
                        }
                    }
                }

                totalItems = Convert.ToInt32(db.GetOuputParams()["totalitems"]);
            }

            return new Tuple<IEnumerable<ProblemAccountLicenseeSiteItem>, IEnumerable<ProblemAccountReason>, int>(problemAccounts, reasons, totalItems);
        }

        private static ProblemAccountLicenseeSiteItem ToProblemAccountLicenseeSiteItem(IDataReader reader)
            => new ProblemAccountLicenseeSiteItem(
                Convert.ToInt32(reader["custid"]),
                reader["site"].ToString(),
                Convert.ToBoolean(reader["isacknowledgement"]),
                reader["username"].ToString(),
                reader["username2"].ToString(),
                reader["currency"].ToString(),
                Convert.ToBoolean(reader["isvip"]),
                reader["actionname"].ToString(),
                ToNullableDecimal(reader["maxbet"]),
                ToNullableDecimal(reader["maxpermatch"]),
                Convert.ToDateTime(reader["markdate"]),
                reader["comment"].ToString(),
                reader["inform1"].ToString(),
                reader["inform2"].ToString(),
                ToNullableInt32(reader["abnormalbetnumber"]));

        protected override ProblemAccountReason ToProblemAccountReason(IDataReader reader)
            => new ProblemAccountReason(
                Convert.ToInt32(reader["custid"]),
                string.Empty,
                Convert.ToDateTime(reader["createddate"]),
                reader["reasonname"].ToString());
    }
}
