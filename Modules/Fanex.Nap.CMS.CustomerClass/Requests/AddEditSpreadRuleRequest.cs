namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    public class AddEditSpreadRuleRequest
    {
        public int PolicyID { get; set; }
        public string PolicyTitle { get; set; }
        public int SportType { get; set; }
        public decimal Spread { get; set; }
        public string CustomerClass { get; set; }
        public string Currencies { get; set; }
        public string Sites { get; set; }
        public string Platforms { get; set; }
        public bool Enable { get; set; }
    }
    
}
