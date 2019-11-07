namespace Fanex.Nap.CMS.Infrastructure.Web
{
    using Logging.Extensions.Web;

    internal class ExceptionHandlerAdapter
    {
        public virtual LoggingConfiguration UseConfig(LoggingConfiguration loggingConfiguration)
            => ExceptionHandler.UseConfig(loggingConfiguration);
    }
}
