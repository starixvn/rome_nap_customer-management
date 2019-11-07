namespace Fanex.Nap.CMS.Infrastructure.Web
{
    using Logging;

    internal class LogManagerAdapter : LogManager
    {
        public new virtual LogManager SetDefaultLogCategory(string defaultCategory)
            => LogManager.SetDefaultLogCategory(defaultCategory);
    }
}
