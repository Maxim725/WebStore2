using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Schema;

namespace WebStore.Logger
{
    public static class Log4NetExtensions
    {
        public static Microsoft.Extensions.Logging.ILoggerFactory AddLog4Net(this Microsoft.Extensions.Logging.ILoggerFactory factory, 
            string configuration = "log4net.config")
        {
            if (Path.IsPathRooted(configuration))
            {
                var assembly = Assembly.GetEntryAssembly() ??
                    throw new InvalidOperationException("Не улаось определить сборку");
                var dir = Path.GetDirectoryName(assembly.Location) ??
                    throw new InvalidOperationException("Не удалось определить папку со сборкой");

                configuration = Path.Combine(dir, configuration);
            }
            factory.AddProvider(new Log4NetLoggerProvider(configuration));
            return factory;
        }
    }
}
