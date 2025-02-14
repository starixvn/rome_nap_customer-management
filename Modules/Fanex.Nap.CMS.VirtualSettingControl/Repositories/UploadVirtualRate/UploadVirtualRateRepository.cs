namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.UploadVirtualRate
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.UploadVirtualRate;
    using Models.UploadVirtualRate;

    public class UploadVirtualRateRepository : IUploadVirtualRateRepository
    {
        public ExcelErrorModel VerifyExcelTemplate(VerifyVirtualRateExcelTemplateCriteria criteria)
        {
            IList<ExcelErrorDTO> excelErrors = new List<ExcelErrorDTO>();
            const int commandTimeoutInMinutes = 60 * 10;

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey(), commandTimeoutInMinutes))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        excelErrors.Add(GetExcelErrorDTO(reader));
                    }
                }

                var outputParams = db.GetOutputParams();

                return new ExcelErrorModel(excelErrors, criteria.ValidUser_Count, 0, null);
            }
        }

        private static ExcelErrorDTO GetExcelErrorDTO(IDataReader reader)
        {
            var username = reader["username"].ToString();

            return new ExcelErrorDTO(
                reader["cellid"].ToString(),
                username,
                username,
                reader["reason"].ToString());
        }

        public Result ExecuteNonQuery(CommandBaseCriteria criteria)
        {
            const int commandTimeoutInMinutes = 60 * 60;

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey(), commandTimeoutInMinutes))
            {
                db.ExecuteNonQuery(criteria);

                return new Result(criteria.ErrCode, criteria.ErrMsg);
            }
        }
    }
}
