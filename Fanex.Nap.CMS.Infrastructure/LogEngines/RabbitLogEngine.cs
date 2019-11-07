namespace Fanex.Nap.CMS.Infrastructure.LogEngines
{
    using Configurations;
    using Logging;
    using Logging.RabbitMQ;

    internal class RabbitLogEngine : LogEngineFactory
    {
        public ILogEngine Create()
            => new RabbitMQLogging(AppSettings.RabbitMQConnectionStrings);
    }
}
