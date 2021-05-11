namespace Fanex.Nap.CMS.Customers.Constants.VirtualRateEngine
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public static class VREngineDayOfWeek
    {
        public const byte Sunday = 1;
        public const byte Monday = 2;
        public const byte Tuesday = 3;
        public const byte Wednesday = 4;
        public const byte Thursday = 5;
        public const byte Friday = 6;
        public const byte Saturday = 7;

        public static IEnumerable<IdNameModel> DayOfWeeks
            => new[]
            {
                new IdNameModel { Id = Monday, Name = nameof(Monday) },
                new IdNameModel { Id = Tuesday, Name = nameof(Tuesday) },
                new IdNameModel { Id = Wednesday, Name = nameof(Wednesday) },
                new IdNameModel { Id = Thursday, Name = nameof(Thursday) },
                new IdNameModel { Id = Friday, Name = nameof(Friday) },
                new IdNameModel { Id = Saturday, Name = nameof(Saturday) },
                new IdNameModel { Id = Sunday, Name = nameof(Sunday) }
            };
    }
}
