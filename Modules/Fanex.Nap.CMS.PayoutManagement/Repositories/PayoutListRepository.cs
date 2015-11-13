namespace Fanex.Nap.CMS.PayoutManagement.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.PayoutManagement.Models;

    public class PayoutListRepository : IPayoutListRepository
    {
        private const int DefaultListCapacity = 32;

        public void GetSitesLicenseesProducts(
            ICriteria criteria,
            out IList<GroupIdNameModel> sitesLicensees,
            out IList<IdNameModel> products)
        {
            string group;
            GroupIdNameModel value;
            IDictionary<string, GroupIdNameModel> sitesLicenseesHash = new Dictionary<string, GroupIdNameModel>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        group = Converter.ToString2(reader["SiteControl"]);
                        if (!sitesLicenseesHash.TryGetValue(group, out value))
                        {
                            value = new GroupIdNameModel 
                            {
                                GroupName = group,
                                ChildItems = new List<IdNameModel>(DefaultListCapacity)
                            };

                            sitesLicenseesHash.Add(group, value);    
                        }

                        value.ChildItems.Add(ToSiteLicensee(reader));
                    }

                    sitesLicensees = sitesLicenseesHash.Values.ToList();
                    products = new List<IdNameModel>(DefaultListCapacity);

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            products.Add(ToProduct(reader));
                        }   
                    }
                }
            }
        }

        public IList<PayoutItem> GetPayoutList(ICriteria criteria)
        {
            IList<PayoutItem> payouts = new List<PayoutItem>(DefaultListCapacity);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        payouts.Add(ToPayoutItem(reader));
                    }    
                }
            }

            return payouts;
        }

        public PayoutDetail GetPayoutDetail(ICriteria criteria, bool viewSecurityCode)
        {
            PayoutDetail payoutDetail = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        payoutDetail = ToPayoutDetail(reader, viewSecurityCode);

                        if (reader.NextResult())
                        {
                            payoutDetail.PayoutNotes = new List<PayoutNote>(DefaultListCapacity);
                            while (reader.Read())
                            {
                                payoutDetail.PayoutNotes.Add(ToPayoutNote(reader));
                            }    
                        }
                    }
                }
            }

            return payoutDetail;
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

        private IdNameModel ToSiteLicensee(IDataReader reader)
        {
            return new IdNameModel
            {
                Id = Converter.ToInt(reader["SiteId"]),
                Name = Converter.ToString2(reader["SiteName"])
            };
        }

        private IdNameModel ToProduct(IDataReader reader)
        {
            return new IdNameModel
            {
                Id = Converter.ToInt(reader["ProductId"]),
                Name = Converter.ToString2(reader["ProductName"])
            };
        }

        private PayoutItem ToPayoutItem(IDataReader reader) 
        {
            return new PayoutItem
            {
                Id = Converter.ToInt(reader["payouttransid"]),
                Product = new IdNameModel { Id = Converter.ToInt(reader["productid"]), Name = Converter.ToString2(reader["productname"]) },
                Member = new IdNameModel { Id = Converter.ToInt(reader["custid"]), Name = Converter.ToString2(reader["username"]) },
                Site = Converter.ToString2(reader["sitename"]),
                Date = Convert.ToDateTime(reader["winlostdate"]),
                RefNo = Converter.To<long>(reader["refno"]),
                Currency = new IdNameModel { Id = Converter.ToInt(reader["currency"]), Name = Converter.ToString2(reader["currencyname"]) },
                Amount = Converter.ToDecimal(reader["winamount"]),
                Status = new IdNameModel { Id = Converter.ToInt(reader["payoutstatusid"]), Name = Converter.ToString2(reader["payoutstatus"]) },
                HasNotes = Converter.ToBool(reader["isnoted"])
            };
        }

        private PayoutDetail ToPayoutDetail(IDataReader reader, bool viewSecurityCode)
        {
            return new PayoutDetail
            {
                RefNo = Converter.To<long>(reader["RefNo"]),
                TransId3rd = Converter.ToString2(reader["TransId3rd"]),
                Site = Converter.ToString2(reader["SiteName"]),
                SecurityCode = viewSecurityCode ? Converter.ToString2(reader["PayoutCode"]) : null,
                Amount = Converter.ToDecimal(reader["WinAmount"]),
                Currency = Converter.ToString2(reader["CurrencyName"])
            };
        }

        private PayoutNote ToPayoutNote(IDataReader reader)
        {
            return new PayoutNote 
            {
                Date = Converter.ToDateTime(reader["createddate"]),
                Status = Converter.ToString2(reader["payoutstatus"]),
                Note = Converter.ToString2(reader["payoutnote"]),
                Handler = Converter.ToString2(reader["username"])
            };
        }
    }
}
