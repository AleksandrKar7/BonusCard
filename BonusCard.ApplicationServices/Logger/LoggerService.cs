using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;

namespace BonusCardManager.ApplicationServices.Logger
{
    public static class LoggerService
    {
        public static void Initialize()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
