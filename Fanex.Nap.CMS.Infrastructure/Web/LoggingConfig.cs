namespace Fanex.Nap.CMS.Infrastructure.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using LogEngines;
    using Logging;
    using Logging.Extensions.Web;

    public static class LoggingConfig
    {
        public static void Init(IEnumerable<string> sensitiveInfo = null, Func<Exception, IDictionary<string, object>> getDbExceptionInfo = null)
        {
            var libs = AppSettings.LogLibs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var logEngines = libs.Select(CreateLogLib).Where(engine => engine != null);
            var logDefaultCategory = AppSettings.LoggingDefaultCategory;

            GetLogManagerAdapter().SetDefaultLogCategory(logDefaultCategory).Use(logEngines);

            var regexSensitiveInfo = GetRegexToHideSensitiveInfo(sensitiveInfo ?? Enumerable.Empty<string>());
            var loggingConfiguration = GetLoggingConfiguration();
            loggingConfiguration = loggingConfiguration
                .GetHttpHeaderInfo()
                .GetHttpSessionInfo()
                .HideSensitiveInfo(regexSensitiveInfo);

            if (getDbExceptionInfo != null)
            {
                loggingConfiguration = loggingConfiguration.UseGetDbExceptionInfo(getDbExceptionInfo);
            }

            GetExceptionHandlerAdapter().UseConfig(loggingConfiguration);
        }

        private static ILogEngine CreateLogLib(string libName)
        {
            var type = Type.GetType($"Fanex.Nap.CMS.Infrastructure.LogEngines.{libName}LogEngine");
            if (type == null)
            {
                return null;
            }

            var engine = Activator.CreateInstance(type) as LogEngineFactory;

            return engine?.Create();
        }

        private static List<string> GetRegexToHideSensitiveInfo(IEnumerable<string> sensitiveInfo)
        {
            var result = new List<string>();
            sensitiveInfo?.ToList().ForEach(info =>
            {
                result.Add($"(?:{info}=)(?<paramValue>[^&|^\\s]+)");
                result.Add($"(\"{info}\":)(?<paramValue>[^,|^\r]+)");
            });

            return result;
        }

        private static LogManagerAdapter GetLogManagerAdapter() => new LogManagerAdapter();

        private static LoggingConfiguration GetLoggingConfiguration() => new LoggingConfiguration();

        private static ExceptionHandlerAdapter GetExceptionHandlerAdapter() => new ExceptionHandlerAdapter();
    }
}
