using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria
{
    public class NonQueryCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
        {
            return UserId > 0 && !string.IsNullOrEmpty(IP);
        }
    }
}
