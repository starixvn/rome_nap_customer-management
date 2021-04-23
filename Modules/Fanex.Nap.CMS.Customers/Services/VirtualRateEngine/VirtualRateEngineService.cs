using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Services.VirtualRateEngine
{
    public class VirtualRateEngineService : IVirtualRateEngineService
    {
        public IEnumerable<IdNameModel> GetCustomerClasses()
        {
            yield return new IdNameModel { Id = 1, Name = "Group Betting" };
            yield return new IdNameModel { Id = 2, Name = "Hedging" };
            yield return new IdNameModel { Id = 3, Name = "Fixed" };
        }

        public IEnumerable<IdNameModel> GetSports()
        {
            yield return new IdNameModel { Id = 1, Name = "Soccer" };
            yield return new IdNameModel { Id = 2, Name = "Basket Ball" };
        }

        public IEnumerable<IdNameModel> GetBetTypes()
        {
            yield return new IdNameModel { Id = 1, Name = "Handicap" };
            yield return new IdNameModel { Id = 2, Name = "Over/Under" };
        }

        public IEnumerable<IdNameModel> GetBetfromList()
        {
            yield return new IdNameModel { Id = 1, Name = "p (phone betting)" };
            yield return new IdNameModel { Id = 2, Name = "z (china desktop)" };
        }

        public IEnumerable<IdNameModel> GetLeagueGroups()
        {
            yield return new IdNameModel { Id = 1, Name = "aaaaaaaaaa" };
            yield return new IdNameModel { Id = 2, Name = "bbbbbbbbbb" };
        }

        public IEnumerable<IdNameModel> GetLeagues()
        {
            yield return new IdNameModel { Id = 1, Name = "cccccccccc" };
            yield return new IdNameModel { Id = 2, Name = "dddddddddd" };
        }
    }
}
