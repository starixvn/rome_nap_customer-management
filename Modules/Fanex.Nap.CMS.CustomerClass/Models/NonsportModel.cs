using System;

namespace Fanex.Nap.CMS.CustomerClass.Models
{
    public class NonsportModel
    {
        public int PolicyId { get; set; }
        public string PolicyTitle { get; set; }
        public string CustomerClass { get; set; }
        public string Sites { get; set; }
        public string Products { get; set; }
        public int MaxBet { get; set; }
        public bool IsEnable { get; set; }
        public byte IsTest { get; set; }
        public int LastModifiedUser { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
