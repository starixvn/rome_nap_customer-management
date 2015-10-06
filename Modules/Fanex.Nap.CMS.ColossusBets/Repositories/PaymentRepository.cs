namespace Fanex.Nap.CMS.ColossusBets.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.ColossusBets.Criteria;
    using Fanex.Nap.CMS.ColossusBets.Models;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class PaymentRepository : IPaymentRepository
    {
        private const int DefaultListCapacity = 32;

        public IList<Ticket> GetTickets(GetTicketsCriteria criteria)
        {
            IList<Ticket> tickets = new List<Ticket>(DefaultListCapacity);
            bool showSecurityCode = criteria.ShowSecurityCode;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        tickets.Add(ToTicket(reader, showSecurityCode));
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

        private Ticket ToTicket(IDataReader reader, bool showSecurityCode) 
        {
            return new Ticket
            {
                Member = new IdNameModel { Id = Converter.ToInt(reader["custid"]), Name = Converter.ToString2(reader["custname"]) },
                Currency = new IdNameModel { Id = Converter.ToInt(reader["currencyid"]), Name = Converter.ToString2(reader["currencyname"]) },
                Site = new IdNameModel { Id = Converter.ToInt(reader["siteid"]), Name = Converter.ToString2(reader["sitename"]) },
                BetId = Converter.ToString2(reader["betid"]),
                TotalPayout = Converter.ToDecimal(reader["totalpayout"]),
                Status = Converter.ToString2(reader["status"]),
                SecurityCode = showSecurityCode ? Converter.ToString2(reader["securitycode"]) : null
            };
        }

        private TicketDetail ToTicketDetail(IDataReader reader)
        {
            return new TicketDetail 
            {
                TransDate = Converter.ToDateTime(reader["transdate"]),
                BetType = Converter.ToInt(reader["bettype"]),
                Sold = Converter.ToDecimal(reader["sold"]),
                WinLoss = Converter.ToDecimal(reader["winlost"]),
                Status = Converter.ToString2(reader["status"])
            };
        }

        private TicketNote ToTicketNote(IDataReader reader)
        {
            DateTime date = Converter.ToDateTime(reader["date"]);
            return new TicketNote 
            {
                Date = date.ToShortFormatedDateWithSlash(),
                Author = Converter.ToString2(reader["author"]),
                Description = Converter.ToString2(reader["note"]),
            };
        }
    }
}
