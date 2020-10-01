using log4net;
using log4net.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly ILog _log;
        public Log4NetLogger(string category, XmlElement xmlConfiguration)
        {
            var loggerRepository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            _log = LogManager.GetLogger(loggerRepository.Name, category);

            log4net.Config.XmlConfigurator.Configure(loggerRepository, xmlConfiguration);

        }

        public IDisposable BeginScope<TState>(TState state)
        {
            // Не поддерживает области журнала
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                case LogLevel.None:
                    return false;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;

                case LogLevel.Information:
                    return _log.IsInfoEnabled;

                case LogLevel.Warning:
                    return _log.IsWarnEnabled;

                case LogLevel.Error:
                    return _log.IsErrorEnabled;

                case LogLevel.Critical:
                    return _log.IsFatalEnabled;


            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, 
                TState state, Exception exception, 
                Func<TState, Exception, string> formatter)
        {
            if (formatter is null)
                throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(logLevel)) return;

            var logMessage = formatter(state, exception);

            if (string.IsNullOrEmpty(logMessage) && exception is null) return;

            switch (logLevel)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                case LogLevel.None:
                    break;
                
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug(logMessage);
                    break;
                case LogLevel.Information:
                    _log.Info(logMessage);
                    break;
                case LogLevel.Warning:
                    _log.Warn(logMessage);
                    break;
                case LogLevel.Error:
                    _log.Error(logMessage);
                    break;
                case LogLevel.Critical:
                    _log.Fatal(logMessage);
                    break;
            }
        }
    }
}
