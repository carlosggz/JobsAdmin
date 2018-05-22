using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Framework.Configuration
{
    public static class SystemConfiguration
    {
        public static int SchedulerTimeInSeconds { get; private set; }

        static SystemConfiguration()
        {
            var settings = System.Configuration.ConfigurationManager.AppSettings;
            SchedulerTimeInSeconds = int.TryParse(settings["TimeInSeconds"], out int seconds) && seconds > 0 ? seconds : 30;
        }
    }
}
