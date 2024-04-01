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
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.Framework.Data;

    internal interface ISiteDataAccess : IBaseRepository
    {
        Tuple<int, IEnumerable<SiteSetting>> GetPagedSiteSetting(ICriteria criteria);

        IEnumerable<SiteSetting> GetSiteSettings(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria, string successfullyMessage = null);

        ProgressResponse GetUploadProgress(GetSiteUploadProgressCriteria criteria);
    }

    internal class SiteDataAccess : BaseRepository, ISiteDataAccess
    {
        public Tuple<int, IEnumerable<SiteSetting>> GetPagedSiteSetting(ICriteria criteria)
            => ExecuteReaderPaging(criteria, ToSitesSetting);

        public IEnumerable<SiteSetting> GetSiteSettings(ICriteria criteria)
            => ExecuteReader(criteria, ToSiteSetting);

        public Result ExecuteNonQuery(ICriteria criteria, string successfullyMessage = null)
        {
            const int commandTimeoutInMinutes = 40 * 60;

            using (var db = new ObjectDb(criteria.GetSettingKey(), commandTimeoutInMinutes))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                var code = Converter.ToInt(outputParams["errcode"]);
                var message = code == 0
                    ? successfullyMessage ?? Converter.ToString2(outputParams["errmsg"])
                    : Converter.ToString2(outputParams["errmsg"]);

                return new Result { Code = code, Message = message };
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
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                var countBatch = Converter.ToInt(outputParams["countbatch"]);
                var initialBatch = Converter.ToInt(outputParams["initialbatch"]);

                return new ProgressResponse(countBatch, initialBatch, 0, string.Empty);
            }
        }
    }
}
