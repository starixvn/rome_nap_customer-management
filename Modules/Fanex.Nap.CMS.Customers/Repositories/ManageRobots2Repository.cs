namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.Customers.Criteria.ManageRobots2;
    using Fanex.Nap.CMS.Customers.Models;
    using Fanex.Nap.CMS.Customers.Models.ManageRobots2;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class ManageRobots2Repository : IManageRobots2Repository
    {
        public RobotSitesModel GetSites(GetSitesCriteria criteria)
        {
            var sites = new List<SiteModel>();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    sites.Add(new SiteModel
                    {
                        SiteId = Converter.ToInt(reader["siteid"]),
                        SiteName = Converter.ToString2(reader["site"]),
                        IsDeposit = Converter.ToBool(reader["IsDeposit"])
                    });
                }

                reader.Close();
            }

            return new RobotSitesModel
            {
                Credits = sites.Where(s => !s.IsDeposit).ToList(),
                Licensees = sites.Where(s => s.IsDeposit).ToList()
            };
        }

        public RobotReport GetRobots(GetRobotsCriteria criteria)
        {
            var result = new RobotReport
            {
                ResultCode = 0,
                ResultMessage = string.Empty,
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate
            };

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result.Robots.Add(new RobotModel
                    {
                        CustId = Converter.ToInt(reader["custId"]),
                        Username = Converter.ToString2(reader["username"]),
                        Level = Converter.ToString2(reader["level"]),
                        Site = Converter.ToString2(reader["site"]),
                        LastChangeTime = reader["LastChangeTime"] == DBNull.Value ? (DateTime?)null : Converter.ToDateTime(reader["LastChangeTime"]),
                        Counter = reader["counter"] == DBNull.Value ? (int?)null : Converter.ToInt(reader["counter"]),
                        ServerIP = Converter.ToString2(reader["ServerIP"]),
                        AddedBy = Converter.ToString2(reader["AddedBy"]),
                        AddedDate = reader["AddedDate"] == DBNull.Value ? (DateTime?)null : Converter.ToDateTime(reader["AddedDate"]),
                        Remark = Converter.ToString2(reader["Remark"]),
                        IP = Converter.ToString2(reader["IP"]),
                        Country = Converter.ToString2(reader["Country"]),
                        Region = Converter.ToString2(reader["Region"]),
                        Currency = Converter.ToString2(reader["Currency"])
                    });
                }

                reader.Close();

                var output = objDb.GetOuputParams();
                result.TotalItems = Converter.ToInt(output["totalitems"]);
            }

            return result;
        }

        public Result DeleteRobot(DeleteRobotCriteria criteria)
        {
            var result = new Result();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                objDb.ExecuteNonQuery();

                var output = objDb.GetOuputParams();
                result.Code = Converter.ToInt(output["errorcode"]);
                result.Message = Converter.ToString2(output["errmsg"]);
            }

            return result;
        }

        public Result AddRobot(AddRobotCriteria criteria)
        {
            var result = new Result();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                objDb.ExecuteNonQuery();

                var output = objDb.GetOuputParams();
                result.Code = Converter.ToInt(output["errcode"]);
                result.Message = Converter.ToString2(output["errmsg"]).TrimEnd(',');
            }

            return result;
        }

        public IEnumerable<CurrencyModel> GetCurrencies(GetCurrenciesCriteria criteria)
        {
            var currencies = new List<CurrencyModel>();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    currencies.Add(new CurrencyModel
                    {
                        CurrencyId = Converter.ToInt(reader["exchangeid"]),
                        CurrencyName = Converter.ToString2(reader["currency"])
                    });
                }

                reader.Close();
            }

            return currencies;
        }
    }
}