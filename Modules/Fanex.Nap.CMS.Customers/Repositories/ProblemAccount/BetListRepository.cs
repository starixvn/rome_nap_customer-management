namespace Fanex.Nap.CMS.Customers.Repositories.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Data.Repository;
    using Models.BetList;

    public class BetListRepository : IBetListRepository
    {
        public Tuple<int, IEnumerable<AbnormalBetTicket>> GetAbnormalBetTickets(ICriteria criteria)
        {
            IList<AbnormalBetTicket> abnormalBetTickets = new List<AbnormalBetTicket>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        abnormalBetTickets.Add(ToAbnormalBetTicket(reader));
                    }
                }

                return new Tuple<int, IEnumerable<AbnormalBetTicket>>(
                    Convert.ToInt32(db.GetOutputParams()["totalrows"]),
                    abnormalBetTickets);
            }
        }

        public int GetTotalSporksbookBetTickets(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteScalar(criteria);

                return Convert.ToInt32(db.GetOutputParams()["totalbettickets"]);
            }
        }

        private static AbnormalBetTicket ToAbnormalBetTicket(IDataReader reader)
            => new AbnormalBetTicket(
                Convert.ToDateTime(reader["createddate"]),
                Convert.ToInt64(reader["transid"]),
                reader["abnormalreason"].ToString());

    }
}
