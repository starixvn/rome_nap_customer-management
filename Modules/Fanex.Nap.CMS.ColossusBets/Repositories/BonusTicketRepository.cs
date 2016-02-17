namespace Fanex.Nap.CMS.ColossusBets.Repositories
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Data;
    using Data.Repository;
    using Infrastructure.Models;
    using Models;

    public class BonusTicketRepository : IBonusTicketRepository
    {
        public BonusTicketDetail GetBonusTicketDetail(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.QueryEntity<BonusTicketDetail>(criteria.GetSpParams());
            }
        }

        public BonusTicketDetail GetBonusTicketDetailByRefNo(ICriteria criteria)
        {
            BonusTicketDetail bonusTicketDetail = new BonusTicketDetail();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        return new BonusTicketDetail
                        {
                            RefNo = Converter.ToString2(reader["refno"]),
                            Pool = Converter.ToString2(reader["poolname"]),
                            CustId = Converter.ToInt(reader["custid"]),
                            Username = Converter.ToString2(reader["username"]),
                            TranId3rd = Converter.ToString2(reader["tranid3rd"]),
                            EventId = Converter.ToString2(reader["eventid"])
                        };
                    }
                }
            }

            return null;
        }

        public IEnumerable<BonusTicket> GetBonusTickets(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<BonusTicket>(criteria.GetSpParams());
            }
        }

        public Result SaveBonusTicket(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var outputParam = db.GetOuputParams();
                if (outputParam != null)
                {
                    return new Result
                    {
                        Code = Converter.ToInt(outputParam["errcode"]),
                        Message = Converter.ToString2(outputParam["errmsg"])
                    };
                }
            }

            return null;
        }
    }
}
