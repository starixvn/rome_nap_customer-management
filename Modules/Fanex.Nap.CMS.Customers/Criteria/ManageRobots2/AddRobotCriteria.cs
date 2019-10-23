namespace Fanex.Nap.CMS.Customers.Criteria.ManageRobots2
{
    using System;
    using System.Collections;
    using System.Linq;
    using Fanex.Data.Repository;

    public class AddRobotCriteria : CriteriaBase
    {
        public string Usernames { get; set; } = string.Empty;

        public string Remark { get; set; } = string.Empty;

        public int Level { get; set; }

        public bool ConfirmUpline { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey()
            => "NAP_Customer_RobotAccountNew_Ins";

        public override Hashtable GetSpParams()
        {
            var result = new Hashtable();

            var usernames = Usernames.Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());
            result.Add("usernames", string.Join(",", usernames));

            result.Add("remark", Remark);
            result.Add("level", Level);
            result.Add("confirmupline", ConfirmUpline);

            result.Add("userid", UserId);
            result.Add("ip", IP);
            result.Add("errcode", ErrCode);
            result.Add("errmsg", ErrMsg);

            return result;
        }
    }
}
