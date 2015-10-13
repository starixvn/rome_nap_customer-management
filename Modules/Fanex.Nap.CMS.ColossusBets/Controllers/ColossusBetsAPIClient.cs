namespace Fanex.Nap.CMS.ColossusBets.Controllers
{
    using System.Net;
    using Fanex.Nap.CMS.ColossusBets.Constants;
    using Fanex.Nap.CMS.ColossusBets.Models.API;
    using Newtonsoft.Json;

    public static class ColossusBetsAPIClient
    {
        private const string AuthorizeURL = "http://122.147.239.73:30051/api/Authorize/AuthorizeCode";

        public static ColossusBetsAuthorization Authorize(int customerId, int memberId)
        {
            WebClient client = new WebClient();

            var queryString = client.QueryString;
            queryString["vendor"] = ColossusBetsConst.ColossusBetsAPIVendor;
            queryString["vendorkey"] = ColossusBetsConst.ColossusBetsAPIVendorKey;
            queryString["vendorproduct"] = ColossusBetsConst.ColossusBetsAPIVendorProduct;
            queryString["customerid"] = customerId.ToString();
            queryString["memberid"] = memberId.ToString();

            string json = client.DownloadString(AuthorizeURL);
            return JsonConvert.DeserializeObject<ColossusBetsAuthorization>(json);
        }
    }
}
