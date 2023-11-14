namespace Fanex.Nap.CMS.DomainManagement.Constants
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.DomainManagement.Resources;

    public static class DomainStatusConst
    {
        private const byte InStock = 2;
        private const byte Available = 1;
        private const byte Active = 3;
        private const byte Unavailable = 0;

        public static IDictionary<byte, string> DomainStatus
            => new Dictionary<byte, string>
            {
                [InStock] = Labels.InStock,
                [Available] = nameof(Available),
                [Active] = nameof(Active),
                [Unavailable] = nameof(Unavailable),
            };
    }
}
