namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    public class AddEditUtilityOfferRequest
    {
        public int PolicyId { get; set; }
        public string PolicyTitle { get; set; }
        public string CustomerClass { get; set; }
        public string Currencies { get; set; }
        public string Sites { get; set; }
        public string UtilityOffering { get; set; }
    }
}
