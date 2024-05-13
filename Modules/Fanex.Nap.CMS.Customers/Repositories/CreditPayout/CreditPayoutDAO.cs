namespace Fanex.Nap.CMS.Customers.Repositories.CreditPayout
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.Data;
    using Fanex.Nap.CMS.Customers.Criteria.CreditPayout;
    using Fanex.Nap.CMS.Customers.Models.CreditPayout;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class CreditPayoutDAO : ICreditPayoutDAO
    {
        private const string MatchPattern = "{0} -vs- {1}";
        private const string FilterMatchPattern = "[{2}] {0} -vs- {1}";
        private const string FormatDate = "MM/dd/yyyy";
        private const string FormatDateTime = "MM/dd/yyyy HH:mm:ss";
        private const int OffsetGMT8GMTMinus4 = 12;
        private readonly NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        public CreditPayoutDAO()
        {
            nfi.NumberDecimalSeparator = ".";
        }

        public IEnumerable<SelectListItem> GetMatches(GetMatchCiretia criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new SelectListItem
                        {
                            Text = string.Format(FilterMatchPattern, reader["Home"], reader["Away"], reader["MatchID"]),
                            Value = reader["MatchID"].ToString()
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
                            Amount = Convert.ToDouble(reader["Amount"]).ToString("0,0", nfi),
                            Site = reader["Site"].ToString(),
                            Username = reader["Username"].ToString(),
                            Currency = reader["Currency"].ToString(),
                            Remark = reader["Remark"].ToString(),
                            Status = reader["Status"].ToString(),
                            MatchID = Convert.ToInt32(reader["MatchID"]),
                            Match = string.Format(MatchPattern, reader["Home"], reader["Away"]),
                            CreatedDate = ExtractDateTimeFromDB(reader["CreatedDate"], FormatDate),
                            ExpiredDate = ExtractDateTimeFromDB(reader["ExpiredDate"], FormatDate),
                            PaymentDate = ExtractDateTimeFromDB(reader["PaymentDate"], FormatDate),
                            IsExpired = IsExpired(reader["ExpiredDate"]),
                            LastModifiedUser = reader["LastModifiedUser"].ToString(),
                            LastModifiedDate = ExtractDateTimeFromDB(reader["LastModifiedDate"], FormatDateTime),
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
                            Amount = Convert.ToDouble(reader["Amount"]).ToString("0,0.##", nfi),
                            Site = reader["Site"].ToString(),
                            Username = reader["Username"].ToString(),
                            Currency = reader["Currency"].ToString(),
                            Match = string.Format(MatchPattern, reader["Home"], reader["Away"]),
                            IsExpired = IsExpired(reader["ExpiredDate"]),
                            ExpiredDate = ExtractDateTimeFromDB(reader["ExpiredDate"], FormatDate)
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

        private static bool IsExpired(object expiredDate)
        {
            if (DBNull.Value == expiredDate)
            {
                return false;
            }

            var timeNow = DateTime.Now.AddHours(OffsetGMT8GMTMinus4);

            return Convert.ToDateTime(expiredDate).Date <= timeNow.Date;
        }

        private static string ExtractDateTimeFromDB(object column, string dateFormat)
        {
            if (DBNull.Value == column)
            {
                return string.Empty;
            }

            return Convert.ToDateTime(column).ToString(dateFormat);
        }
    }
}