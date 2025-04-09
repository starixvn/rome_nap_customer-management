using System.Linq;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Models.WhitelistBigCreditMembers
{
    public class AddMemberResult : Result
    {
        private static readonly int[] ErrorCodes = new[] { 1001, 1002 };
        private static readonly int[] WarningCodes = new[] { 1003, 1004, 1005 };

        public AddMemberResult()
        {
        }


        public AddMemberResult(string username, int code, string message)
            : base(code, message)
        {
            Username = username;
        }

        public string Username { get; set; }

        public bool IsError => ErrorCodes.Contains(Code);

        public bool IsWarning => WarningCodes.Contains(Code);
    }
}
