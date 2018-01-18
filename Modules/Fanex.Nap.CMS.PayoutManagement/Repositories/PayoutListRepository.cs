namespace Fanex.Nap.CMS.PayoutManagement.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.PayoutManagement.Models;

    public class PayoutListRepository : IPayoutListRepository
    {
        public void GetSitesLicenseesProducts(
            ICriteria criteria,
            out IList<GroupIdNameModel> sitesLicensees,
            out IList<ProductModel> products)
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
                        group = reader["SiteControl"].ToString();

                        if (!sitesLicenseesHash.TryGetValue(group, out value))
                        {
                            value = new GroupIdNameModel 
                            {
                                GroupName = group,
                                ChildItems = new List<IdNameModel>(32)
                            };

                            sitesLicenseesHash.Add(group, value);    
                        }

                        value.ChildItems.Add(ToSiteLicensee(reader));
                    }

                    sitesLicensees = sitesLicenseesHash.Values.ToList();
                    products = new List<ProductModel>();

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

        public IEnumerable<PayoutItem> GetPayoutList(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return ToPayoutItem(reader);
                    }    
                }
            }
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
                            payoutDetail.PayoutNotes = new List<PayoutNote>();

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
                Id = Convert.ToInt32(reader["SiteId"]),
                Name = reader["SiteName"].ToString()
            };
        }

        private ProductModel ToProduct(IDataReader reader)
        {
            return new ProductModel
            {
                Id = reader["ProductId"].ToString(),
                Name = reader["ProductName"].ToString()
            };
        }

        private PayoutItem ToPayoutItem(IDataReader reader) 
        {
            return new PayoutItem
            {
                Id = Convert.ToInt32(reader["payouttransid"]),
                Product = new IdNameModel { Id = Convert.ToInt32(reader["productid"]), Name = reader["productname"].ToString() },
                Member = new IdNameModel { Id = Convert.ToInt32(reader["custid"]), Name = reader["username"].ToString() },
                Site = reader["sitename"].ToString(),
                Date = Convert.ToDateTime(reader["winlostdate"]),
                RefNo = Convert.ToInt64(reader["refno"]),
                Currency = new IdNameModel { Id = Convert.ToInt32(reader["currency"]), Name = reader["currencyname"].ToString() },
                Amount = Convert.ToDecimal(reader["winamount"]),
                Status = new IdNameModel { Id = Convert.ToInt32(reader["payoutstatusid"]), Name = reader["payoutstatus"].ToString() },
                HasNotes = Convert.ToBoolean(reader["isnoted"])
            };
        }

        private PayoutDetail ToPayoutDetail(IDataReader reader, bool viewSecurityCode)
        {
            return new PayoutDetail
            {
                RefNo = Convert.ToInt64(reader["RefNo"]),
                TransId3rd = reader["TransId3rd"].ToString(),
                CustomerId = Convert.ToInt32(reader["CustId"]),
                ProductId = Convert.ToInt32(reader["ProductId"]),
                Site = reader["SiteName"].ToString(),
                SecurityCode = viewSecurityCode ? reader["PayoutCode"].ToString() : null,
                Amount = Convert.ToDecimal(reader["WinAmount"]),
                Currency = reader["CurrencyName"].ToString()
            };
        }

        private PayoutNote ToPayoutNote(IDataReader reader)
        {
            return new PayoutNote 
            {
                Date = Convert.ToDateTime(reader["createddate"]),
                Status = reader["payoutstatus"].ToString(),
                Note = reader["payoutnote"].ToString(),
                Handler = reader["username"].ToString()
            };
        }
    }
}
