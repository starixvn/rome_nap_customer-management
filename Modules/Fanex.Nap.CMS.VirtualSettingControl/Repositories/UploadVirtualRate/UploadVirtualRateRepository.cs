namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.UploadVirtualRate
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Models.UploadVirtualRate;

    public class UploadVirtualRateRepository : IUploadVirtualRateRepository
    {
        public ExcelErrorModel VerifyExcelTemplate(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var excelErrors = db.Query<ExcelErrorDTO>(criteria.GetSpParams());

                return new ExcelErrorModel { ValidSetting = 0, ExcelErrors = excelErrors };
            }
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
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
