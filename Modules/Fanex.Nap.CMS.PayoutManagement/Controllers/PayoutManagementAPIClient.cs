namespace Fanex.Nap.CMS.PayoutManagement.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Xml;
    using Fanex.Nap.CMS.PayoutManagement.Constants;
    using Fanex.Nap.CMS.PayoutManagement.Models.API;
    using Newtonsoft.Json;

    public static class PayoutManagementAPIClient
    {
        public static async Task<ColossusBetsAuthorization> ColossusBetsAuthorize(int customerId, int memberId)
        {
            using (WebClient client = new WebClient())
            {
                string json = await client.DownloadStringTaskAsync(
                    string.Format(PayoutManagementConst.ColossusBetsAPIAuthorizationURL, customerId, memberId));

                return JsonConvert.DeserializeObject<ColossusBetsAuthorization>(json);
            }
        }

        public static MicrogamingResult MicrogamingResult(string sitename, int customerId, string tranId3rd)
        {
            string objectParams = string.Format("SiteName={0}&CustID={1}&GameID={2}", sitename, customerId, tranId3rd);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string xml = client.UploadString(PayoutManagementConst.MicroGamingResultAPI, "POST", objectParams);
                return XmlToMicrogamingResult(xml);
            }
        }

        private static MicrogamingResult XmlToMicrogamingResult(string xml)
        {
            MicrogamingResult microgamingResult = new MicrogamingResult();
            
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            foreach (XmlNode note in xdoc.ChildNodes[0].ChildNodes)
            {
                string name = note.Name;
                string value = note.InnerXml;
                switch (name)
                {
                    case "URL":
                        microgamingResult.URL = value;
                        break;

                    case "ReturnCode":
                        microgamingResult.ReturnCode = int.Parse(value);
                        break;

                    case "ReturnMessage":
                        microgamingResult.ReturnMessage = value;
                        break;
                }    
            }

            return microgamingResult;
        }
    }
}
