namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.Logging.Extensions.Web;
    using Newtonsoft.Json;

    public interface IProblemAccountCTSService
    {
        void RemoveProblemAccounts(string custIds);
    }

    public class ProblemAccountCTSService : IProblemAccountCTSService
    {
        public void RemoveProblemAccounts(string custIds)
        {
            if (!string.IsNullOrWhiteSpace(custIds))
            {
                var ctsUrl = ConfigurationManager.AppSettings["CTSRemoveProblemAccountsURL"];

                if (!string.IsNullOrWhiteSpace(ctsUrl))
                {
                    Task.Factory.StartNew(() =>
                    {
                        RemoveProblemAccountsBackgroundJob(ctsUrl, custIds);
                    });
                }
            }
        }

        private static void RemoveProblemAccountsBackgroundJob(string url, string custIds)
        {
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(custIds), Encoding.UTF8, "application/json"))
                {
                    try
                    {
                        var result = client.PostAsync(new Uri(url), content).Result;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.Handle(ex, new Dictionary<string, object> { ["RemoveProblemAccounts_CustIds"] = custIds });
                    }
                }
            }
        }
    }
}
