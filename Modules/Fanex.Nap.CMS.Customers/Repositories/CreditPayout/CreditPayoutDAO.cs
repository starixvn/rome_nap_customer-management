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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var result = new List<PayoutPromotion>();
                using (var reader = db.ExecuteReader(criteria))
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

                var ouputParams = db.GetOutputParams();
                criteria.TotalItems = Convert.ToInt32(ouputParams["totalitems"]);

                return result;
            }
        }

        public PayoutPromotion ValidateVerifyCode(VerifySelPayoutPromotionCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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

                    return result;
                }
            }
        }

        public Result UpdatePayoutPromotion(UpdPayoutPromotionCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);

                return new Result(criteria.ErrCode, criteria.ErrMsg);
            }
        }
    }
}