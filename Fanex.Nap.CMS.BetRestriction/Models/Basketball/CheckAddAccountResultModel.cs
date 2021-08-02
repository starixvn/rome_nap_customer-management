using Fanex.Nap.CMS.Infrastructure.Extensions;
using System.Data;

namespace Fanex.Nap.CMS.BetRestriction.Models.Basketball
{
    public class CheckAddAccountResultModel
    {
        public CheckAddAccountResultModel(IDataReader reader, string userName) : this(reader, userName, null)
        { }

        public CheckAddAccountResultModel(IDataReader reader, string userName, ICMSConverter converter)
        {
            ICMSConverter _converter = converter ?? new CMSConverterImplement(); 
            CustId = _converter.ToInt32(reader["CustID"]);
            IsUplineExists = _converter.ToBoolean(reader["IsUplineExists"]);
            UplineLevel = _converter.ToString(reader["UplineLevel"]);
            ExistingUplineUserName = _converter.ToString(reader["ExistingUplineUserName"]);
            UplineSeizeBetTicket = _converter.ToInt32Nullable(reader["Upline_SeizeBetTicket"]);
            IsDownlineExists = _converter.ToBoolean(reader["IsDownlineExists"]);
            MinDownlineSeizeBetTicket = _converter.ToInt32Nullable(reader["Min_Downline_SeizeBetTicket"]);
            UserName = userName;
        }

        public int CustId { get; }

        public string UserName { get; }

        public bool IsUplineExists { get; }

        public string UplineLevel { get; }

        public string ExistingUplineUserName { get; }

        public int? UplineSeizeBetTicket { get; }

        public bool IsDownlineExists { get; }

        public int? MinDownlineSeizeBetTicket { get; }
    }
}
