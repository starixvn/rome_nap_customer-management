namespace Fanex.Nap.CMS.Infrastructure.LogEngines
{
    using Logging;

    internal interface LogEngineFactory
    {
        ILogEngine Create();
    }
}
