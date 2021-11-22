namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Nap.CMS.Customers.Models.ProblemAccount;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Newtonsoft.Json;

    public interface IProblemAccountCTSService
    {
        void RemoveProblemAccounts(string custIds);

        void UpdateProblemAccounts(CTSUpdateProblemAccount accounts);
    }

    public class ProblemAccountCTSService : IProblemAccountCTSService
    {
        private const string RemoveProblemAccountsURL = "/api/Cts/RemoveProblemAccounts";
        private const string UpdateProblemAccountsURL = "/api/Cts/UpdateCategoryToCTSWhenPAUpdated";

        public void RemoveProblemAccounts(string custIds)
        {
            if (!string.IsNullOrWhiteSpace(custIds) && !string.IsNullOrWhiteSpace(AppSettings.CTSAPIURL))
            {
                Task.Factory.StartNew(async () =>
                {
                    await CallCTSAPIAync(
                        $"{AppSettings.CTSAPIURL}{RemoveProblemAccountsURL}",
                        JsonConvert.SerializeObject(custIds),
                        "RemoveProblemAccounts_CustIds");
                });
            }
        }

        public void UpdateProblemAccounts(CTSUpdateProblemAccount accounts)
        {
            if (!string.IsNullOrWhiteSpace(AppSettings.CTSAPIURL))
            {
                Task.Factory.StartNew(async () =>
                {
                    await CallCTSAPIAync(
                        $"{AppSettings.CTSAPIURL}{UpdateProblemAccountsURL}",
                        JsonConvert.SerializeObject(accounts),
                        "UpdateProblemAccounts_Params");
                });
            }
        }

        private static async Task CallCTSAPIAync(string url, string inputParams, string errorCustomDataKey)
        {
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(inputParams, Encoding.UTF8, "application/json"))
                {
                    try
                    {
                        var result = await client.PostAsync(new Uri(url), content);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.Handle(
                            ex,
                            new Dictionary<string, object>
                            {
                                [errorCustomDataKey] = inputParams
                            });
                    }
                }
            }
        }
    }
}
