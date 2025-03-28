using Fanex.Data.Repository;

namespace Fanex.NAP.CMS.BetLimitTypeControl.Criteria
{
    public class DeleteCriteria : NonQueryCommand
    {
        public int BetLimitTypeId { get; set; }

        public string IP { get; set; }

        public int UserId { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public override bool IsValid() => BetLimitTypeId > 0 && UserId > 0 && !string.IsNullOrWhiteSpace(IP);

        public override string GetSettingKey() => "NAP_BLControl_TypeDelete";
    }
}
