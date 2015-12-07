namespace Fanex.Nap.CMS.PayoutManagement.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.PayoutManagement.Models;

    public class BetTicketRepository : IBetTicketRepository
    {
        public BetTicket GetMicrogaming(ICriteria criteria)
        {
            BetTicket microgaming = new BetTicket();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        microgaming.ToBaseBetTicket(reader);
                    }
                }
            }

            return microgaming;
        }

        public ColossusBetsTicket GetColossusBets(ICriteria criteria)
        {
            ColossusBetsTicket colossusbets = new ColossusBetsTicket 
            {
                BetTicketDetails = new List<BetTicketDetail>()
            };

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        colossusbets.BetTicketDetails.Add(ToBetTicketDetail(reader));    
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        colossusbets.ToBaseBetTicket(reader);
                    }
                }
            }

            return colossusbets;
        }

        private BetTicketDetail ToBetTicketDetail(IDataReader reader)
        {
            return new BetTicketDetail
            {
                TransDate = Converter.ToDateTime(reader["transdate"]),
                BetType = Converter.ToInt(reader["bettype"]),
                Sold = Converter.ToDecimal(reader["sold"]),
                WinLoss = Converter.ToDecimal(reader["winlost"]),
                Status = Converter.ToString2(reader["status"])
            };
        }
    }
}
