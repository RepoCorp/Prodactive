using log4net;
using log4net.Repository.Hierarchy;
using log4net.Layout;
using log4net.Appender;
using log4net.Core;

namespace Zeitgeist.Appsco.Web.Helpers
{
    public static class Logger
    {
//tarea: definir diferentes configuraciones del log4net, mientras mas dinamico mejor
        public static void Setup()
        {
            Hierarchy hierarchy = (Hierarchy) LogManager.GetRepository();
            PatternLayout patternLayout = new PatternLayout();
            //patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ConversionPattern = "%date %level %logger - %message%newline";
            patternLayout.ActivateOptions();
            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = false;
            roller.File = @"Logs/prodactive.log";
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 20;
            roller.MaximumFileSize = "1MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);
            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }
    }
}