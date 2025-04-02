namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System;

    public class UtilityOfferingModel
    {
        public int PolicyId { get; set; }
        public string PolicyTitle { get; set; }
        public string CustomerClass { get; set; }
        public string Currencies { get; set; }
        public string Sites { get; set; }
        public string UtilityOffering { get; set; }
        public byte IsTest { get; set; }
        public int LastModifiedUser { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
