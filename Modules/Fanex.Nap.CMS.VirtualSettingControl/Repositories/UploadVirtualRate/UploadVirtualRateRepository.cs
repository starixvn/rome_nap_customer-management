namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.UploadVirtualRate
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Models.UploadVirtualRate;

    public class UploadVirtualRateRepository : IUploadVirtualRateRepository
    {
        public ExcelErrorModel VerifyExcelTemplate(ICriteria criteria)
        {
            IList<ExcelErrorDTO> excelErrors = new List<ExcelErrorDTO>();
            const int commandTimeoutInMinutes = 60 * 10;

            using (var db = new ObjectDb(criteria.GetSettingKey(), commandTimeoutInMinutes))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        excelErrors.Add(GetExcelErrorDTO(reader));
                    }
                }

                var outputParams = db.GetOuputParams();

                return new ExcelErrorModel(excelErrors, Convert.ToInt32(outputParams["validuser_count"]), 0, null);
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

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            const int commandTimeoutInMinutes = 60 * 60;

            using (var db = new ObjectDb(criteria.GetSettingKey(), commandTimeoutInMinutes))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    outputParams["errmsg"].ToString());
            }
        }
    }
}
