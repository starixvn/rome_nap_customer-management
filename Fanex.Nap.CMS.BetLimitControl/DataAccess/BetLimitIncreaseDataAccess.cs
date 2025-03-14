namespace Fanex.NAP.CMS.BetLimitControl.DataAccess
{
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease;
    using Fanex.NAP.CMS.BetLimitControl.Models.BetLimitIncrease;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface IBetLimitIncreaseDataAccess
    {
        IEnumerable<BetLimitIncreaseModel> SearchBetLimitIncrease(SearchBetLimitIncreaseCriteria criteria);
        Tuple<Result, IEnumerable<BetLimitIncreaseModel>> ValidateUsername(CheckBetLimitIncreaseCriteria criteria);
        Result DoAction(CriteriaBase criteria, string successfullyMessage = null);
    }

    public class BetLimitIncreaseDataAccess: BaseRepository, IBetLimitIncreaseDataAccess
    {
        public IEnumerable<BetLimitIncreaseModel> SearchBetLimitIncrease(SearchBetLimitIncreaseCriteria criteria)
        {
            var results = new List<BetLimitIncreaseModel>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        results.Add(MappToBetLimitCheck(reader));
                    }
                }

                return results;
            }
        }

        public Tuple<Result, IEnumerable<BetLimitIncreaseModel>> ValidateUsername(CheckBetLimitIncreaseCriteria criteria)
        {
            var validUsername = new List<BetLimitIncreaseModel>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        validUsername.Add(MappToBetLimitCheck(reader));
                    }
                }

                var outputParams = db.GetOuputParams();

                var code = Converter.ToInt(outputParams["errcode"]);
                var message = Converter.ToString2(outputParams["errmsg"]);

                var result = new Result { Code = code, Message = message };

                return Tuple.Create<Result, IEnumerable<BetLimitIncreaseModel>>(result, validUsername);
            }
        }

        public Result DoAction(CriteriaBase criteria, string successfullyMessage = null)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();
                var outputParams = db.GetOuputParams();

                var code = Converter.ToInt(outputParams["errcode"]);
                var message = code == 0
                    ? successfullyMessage ?? Converter.ToString2(outputParams["errmsg"])
                    : Converter.ToString2(outputParams["errmsg"]);

                return new Result { Code = code, Message = message };
            }
        }

        private BetLimitIncreaseModel MappToBetLimitCheck(IDataReader dataReader)
        {
            return new BetLimitIncreaseModel
            {
                Username = HasColumn(dataReader, "username") && DBNull.Value != dataReader["username"] ? dataReader["username"].ToString() : string.Empty,
                CustId = HasColumn(dataReader, "custid") && DBNull.Value != dataReader["custid"] ? Convert.ToInt32(dataReader["custid"]) : 0,
                Site = HasColumn(dataReader, "Site") && DBNull.Value != dataReader["Site"] ? dataReader["Site"].ToString() : string.Empty,
                Currency = HasColumn(dataReader, "Currency") && DBNull.Value != dataReader["Currency"] ? dataReader["Currency"].ToString() : string.Empty,
                BetType = HasColumn(dataReader, "BetType") && DBNull.Value != dataReader["BetType"] ? Convert.ToInt32(dataReader["BetType"]) : 0,
                SportType = HasColumn(dataReader, "SportType") && DBNull.Value != dataReader["SportType"] ? Convert.ToInt32(dataReader["SportType"]) : 0,
                SiteId = HasColumn(dataReader, "SiteId") && DBNull.Value != dataReader["SiteId"] ? Convert.ToInt16(dataReader["SiteId"]) : 0,
                Multiplier = HasColumn(dataReader, "Multiplier") && DBNull.Value != dataReader["Multiplier"] ? Convert.ToDecimal(dataReader["Multiplier"]) : 0,
                Note = HasColumn(dataReader, "Note") && DBNull.Value != dataReader["Note"] ? dataReader["Note"].ToString() : string.Empty,
                LastModifiedUser = HasColumn(dataReader, "LastModifiedUserName") && DBNull.Value != dataReader["LastModifiedUserName"] ? dataReader["LastModifiedUserName"].ToString() : string.Empty,
                LastModifiedTime = HasColumn(dataReader, "LastModifiedTime") && DBNull.Value != dataReader["LastModifiedTime"] ? Convert.ToDateTime(dataReader["LastModifiedTime"]).ToString("MM/dd/yyyy") : string.Empty
            };
        }


        private static bool HasColumn(IDataRecord dataRecord, string columnName)
        {
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                if (dataRecord.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
