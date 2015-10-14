namespace Fanex.Nap.CMS.ColossusBets.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.ColossusBets.Models;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class PayoutRepository : IPayoutRepository
    {
        private const int DefaultListCapacity = 32;

        public IList<Ticket> GetTickets(ICriteria criteria)
        {
            IList<Ticket> tickets = new List<Ticket>(DefaultListCapacity);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        tickets.Add(ToTicket(reader));
                    }    
                }
            }

            return tickets;
        }

        public IList<TicketDetail> GetTicketDetails(ICriteria criteria)
        {
            IList<TicketDetail> ticketDetails = new List<TicketDetail>(DefaultListCapacity);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        ticketDetails.Add(ToTicketDetail(reader));
                    }
                }
            }

            return ticketDetails;
        }

        public IList<TicketNote> GetTicketNotes(ICriteria criteria)
        {
            IList<TicketNote> ticketNotes = new List<TicketNote>(DefaultListCapacity);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        ticketNotes.Add(ToTicketNote(reader));
                    }
                }
            }

            return ticketNotes;
        }

        public Result UpdateTicketStatus(ICriteria criteria)
        {
            Result result = new Result();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                
                var outputParams = db.GetOuputParams();
                result.Code = (int)outputParams["errcode"];
                result.Message = (string)outputParams["errmsg"];
            }

            return result;
        }

        private Ticket ToTicket(IDataReader reader) 
        {
            return new Ticket
            {
                Number = Converter.ToInt(reader["rowidx"]),
                Member = new IdNameModel { Id = Converter.ToInt(reader["custid"]), Name = Converter.ToString2(reader["username"]) },
                Currency = new IdNameModel { Id = Converter.ToInt(reader["currency"]), Name = Converter.ToString2(reader["currencyname"]) },
                Site = Converter.ToString2(reader["sitename"]),
                BetId = Converter.To<long>(reader["betid"]),
                TotalPayout = Converter.ToDecimal(reader["winamount"]),
                Status = new IdNameModel { Id = Converter.ToInt(reader["statusid"]), Name = Converter.ToString2(reader["paystatus"]) },
                SecurityCode = Converter.ToString2(reader["paycode"]),
                HasNotes = Converter.ToBool(reader["isnoted"])
            };
        }

        private TicketDetail ToTicketDetail(IDataReader reader)
        {
            DateTime transdate = Converter.ToDateTime(reader["transdate"]);
            return new TicketDetail
            {
                Date = transdate.ToLongFormatedDateWithSlash(),
                BetType = Converter.ToInt(reader["bettype"]),
                Sold = Converter.ToDecimal(reader["sold"]),
                WinLoss = Converter.ToDecimal(reader["winlost"]),
                Status = Converter.ToString2(reader["status"])
            };
        }

        private TicketNote ToTicketNote(IDataReader reader)
        {
            DateTime createdDate = Converter.ToDateTime(reader["createddate"]);
            return new TicketNote 
            {
                Date = createdDate.ToLongFormatedDateWithSlash(),
                Author = Converter.ToString2(reader["username"]),
                Note = Converter.ToString2(reader["paynote"]),
            };
        }
    }
}
