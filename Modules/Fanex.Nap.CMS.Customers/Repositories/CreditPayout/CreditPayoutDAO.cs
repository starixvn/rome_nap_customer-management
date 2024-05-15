namespace Fanex.Nap.CMS.Customers.Repositories.CreditPayout
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Nap.CMS.Customers.Criteria.CreditPayout;
    using Fanex.Nap.CMS.Customers.Models.CreditPayout;
    using Fanex.Nap.CMS.Customers.Services.CreditPayout;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class CreditPayoutDAO : ICreditPayoutDAO
    {
        public IEnumerable<Match> GetMatches(GetMatchCiretia criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new Match
                        {
                            MatchID = Convert.ToInt32(reader["MatchID"]),
                            Home = reader["Home"].ToString(),
                            Away = reader["Away"].ToString()
                        };
                    }
                }
            }
        }

        public IEnumerable<PayoutPromotion> SearchPayout(SelPayoutPromotionCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var result = new List<PayoutPromotion>();
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new PayoutPromotion
                        {
                            DrawID = Convert.ToInt32(reader["DrawID"]),
                            CustID = Convert.ToInt32(reader["CustID"]),
                            Amount = CreditPayoutHelper.ExtractDoubleFromDB(reader["Amount"]),
                            Site = reader["Site"].ToString(),
                            Username = reader["Username"].ToString(),
                            Currency = reader["Currency"].ToString(),
                            Remark = reader["Remark"].ToString(),
                            Status = reader["Status"].ToString(),
                            MatchID = Convert.ToInt32(reader["MatchID"]),
                            CreatedDate = CreditPayoutHelper.ExtractDateFromDB(reader["CreatedDate"]),
                            ExpiredDate = CreditPayoutHelper.ExtractDateFromDB(reader["ExpiredDate"]),
                            PaymentDate = CreditPayoutHelper.ExtractDateFromDB(reader["PaymentDate"]),
                            IsExpired = CreditPayoutHelper.IsExpired(reader["ExpiredDate"]),
                            LastModifiedUser = reader["LastModifiedUser"].ToString(),
                            LastModifiedDate = CreditPayoutHelper.ExtractDateTimeFromDB(reader["LastModifiedDate"]),
                        });
                    }
                }

                var ouputParams = db.GetOuputParams();
                criteria.TotalItems = Convert.ToInt32(ouputParams["totalitems"]);

                return result;
            }
        }

        public PayoutPromotion ValidateVerifyCode(VerifySelPayoutPromotionCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    PayoutPromotion result = null;
                    if (reader.Read())
                    {
                        result = new PayoutPromotion
                        {
                            DrawID = Convert.ToInt32(reader["DrawID"]),
                            CustID = Convert.ToInt32(reader["CustID"]),
                            Amount = CreditPayoutHelper.ExtractDoubleFromDB(reader["Amount"]),
                            MatchID = Convert.ToInt32(reader["MatchID"]),
                            Site = reader["Site"].ToString(),
                            Username = reader["Username"].ToString(),
                            Currency = reader["Currency"].ToString(),
                            IsExpired = CreditPayoutHelper.IsExpired(reader["ExpiredDate"]),
                            ExpiredDate = CreditPayoutHelper.ExtractDateFromDB(reader["ExpiredDate"])
                        };
                    }

                    var ouputParams = db.GetOuputParams();
                    criteria.ErrCode = Convert.ToInt32(ouputParams["errcode"]);
                    criteria.ErrMsg = ouputParams["errmsg"]?.ToString();

                    return result;
                }
            }
        }

        public Result UpdatePayoutPromotion(UpdPayoutPromotionCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var ouputParams = db.GetOuputParams();

                return new Result(Convert.ToInt32(DBNull.Value == ouputParams["errcode"]? 0: ouputParams["errcode"]), ouputParams["errmsg"]?.ToString());
            }
        }
    }
}