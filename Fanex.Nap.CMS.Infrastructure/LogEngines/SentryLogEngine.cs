namespace Fanex.Nap.CMS.Infrastructure.LogEngines
{
    using Configurations;
    using Logging;
    using Logging.Sentry.RavenClient;

    internal class SentryLogEngine : LogEngineFactory
    {
        public ILogEngine Create()
        {
            var sentryOptions = new SentryEngineOptions
            {
                Dsn = new SharpRaven.Dsn(AppSettings.SentryBackendUrl),
                FrontEndDsn = new SharpRaven.Dsn(AppSettings.SentryUrl)
            };

            return new SentryLogging(sentryOptions, AppSettings.RavenLibPath);
        }
    }
}
