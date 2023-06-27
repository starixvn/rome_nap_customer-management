using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.BetRestriction.ViewModels
{
    public class EditAccountViewModel
    {
        public int CustId { get; set; }

        public string Account { get; set; }

        public short? LiveBetAccept { get; set; }

        public byte? SeizeBetTicket { get; set; }

        public string LeagueGroupId { get; set; }

        public decimal? MaxBet { get; set; }

        public decimal? MaxPerMatch { get; set; }

        public int? FollowAccountID { get; set; }

        public string FollowAccount { get; set; }

        public byte AccumulateBalance { get; set; }
    }
}
