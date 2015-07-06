namespace Fanex.Nap.CMS.FPSClient.Repositories
{
    using Fanex.Data;
    using Fanex.Nap.CMS.FPSClient.Criteria;

    public class FPSRepository : IFPSRepository
    {
        public void GetCustomerTypeAndSite(GetCustomerTypeAndSiteCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var outputHash = db.GetOuputParams();
                criteria.IsDeposit = (bool)outputHash["isdeposit"];
                criteria.Site = outputHash["sitename"].ToString();
            }
        }
    }
}
