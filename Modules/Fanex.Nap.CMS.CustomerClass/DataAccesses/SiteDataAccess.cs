namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Data.Repository;

    internal interface ISiteDataAccess : IBaseRepository
    {
        Tuple<int, IEnumerable<SiteSetting>> GetPagedSiteSetting(CriteriaBase criteria);

        IEnumerable<SiteSetting> GetSiteSettings(GetDuplicatedSiteSettingCriteria criteria);

        Result ExecuteNonQuery(CommandBaseCriteria criteria, string successfullyMessage = null);

        ProgressResponse GetUploadProgress(GetSiteUploadProgressCriteria criteria);
    }

    internal class SiteDataAccess : BaseRepository, ISiteDataAccess
    {
        private const string LastUpdatedTime = "Fanex.CMS.CCM.Site.LastUpdatedTime-{0}";

        public Tuple<int, IEnumerable<SiteSetting>> GetPagedSiteSetting(CriteriaBase criteria)
            => ExecuteReaderPaging(criteria, ToSitesSetting);

        public IEnumerable<SiteSetting> GetSiteSettings(GetDuplicatedSiteSettingCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        yield return ToSiteSetting(reader);
                    }
                }
            }
        }

        public Result ExecuteNonQuery(CommandBaseCriteria criteria, string successfullyMessage = null)
        {
            const int commandTimeoutInMinutes = 40 * 60;

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey(), commandTimeoutInMinutes))
            {
                db.ExecuteNonQuery(criteria);
                var message = criteria.ErrCode == 0
                    ? successfullyMessage ?? criteria.ErrMsg
                    : criteria.ErrMsg;

                return new Result { Code = criteria.ErrCode, Message = message };
            }
        }

        private static SiteSetting ToSiteSetting(IDataReader reader)
            => new SiteSetting(
                int.Parse(reader["PolicyId"].ToString()),
                reader["PolicyTitle"].ToString(),
                reader["Site"].ToString(),
                reader["CurrencyName"].ToString(),
                string.Empty,
                reader["Platform"].ToString());

        private static SiteSetting ToSitesSetting(IDataReader reader)
            => new SiteSetting(
                int.Parse(reader["PolicyId"].ToString()),
                reader["PolicyTitle"].ToString(),
                reader["Sites"].ToString(),
                reader["CurrencyName"].ToString(),
                reader["CurrencyID"].ToString(),
                reader["Platform"].ToString());

        public ProgressResponse GetUploadProgress(GetSiteUploadProgressCriteria criteria)
        {
            var lastUpdatedTimeKey = string.Format(LastUpdatedTime, criteria.UserID);
            criteria.LastModifiedTime = CacheManager.Get<DateTime?>(lastUpdatedTimeKey);
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);
                if (criteria.CountBatch > 0 && criteria.InitialBatch > 0)
                {
                    if(criteria.LastModifiedTime == null)
                    {
                        var lastUpdatedTime = criteria.LastModifiedTime;
                        CacheManager.Set(lastUpdatedTimeKey, lastUpdatedTime, new TimeSpan(1, 0, 0, 0));
                    }
                } else
                {
                    CacheManager.Remove(lastUpdatedTimeKey);
                }

                return new ProgressResponse(criteria.CountBatch, criteria.InitialBatch);
            }
        }
    }
}
