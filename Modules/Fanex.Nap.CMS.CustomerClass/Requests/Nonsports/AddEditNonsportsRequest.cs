namespace Fanex.Nap.CMS.CustomerClass.Requests.Nonsports
{
    public class AddEditNonsportsRequest
    {
        public int PolicyId { get; set; }
        public string PolicyTitle { get; set; }
        public string CustomerClass { get; set; }
        public string Products { get; set; }
        public string Sites { get; set; }
        public int MaxBet { get; set; }
        public bool IsEnable { get; set; }
    }
}
