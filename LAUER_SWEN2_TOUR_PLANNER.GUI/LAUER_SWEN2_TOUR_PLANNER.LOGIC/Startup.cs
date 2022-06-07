using LAUER_SWEN2_TOUR_PLANNER.DAL;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.LOGIC
{
    public class Startup
    {
        private static readonly Lazy<Startup> startup = new Lazy<Startup>(() => new Startup());

        public static Startup GetInstance { get { return startup.Value; } }

        private static ConfigMapper? configMapper;
        private Startup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("..\\Logs\\Log.txt")
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

            SetupMapper();
        }

        public static ConfigMapper? GetConfigMapper()
        {
            if (configMapper == null)
            {
                SetupMapper();
            }
            return configMapper;
        }
        private static void SetupMapper(string configMapperConfig = "..\\..\\config.json")
        {
            using (var sr = new StreamReader(configMapperConfig))
            {
                try
                {
                    configMapper = JsonSerializer.Deserialize<ConfigMapper>(sr.ReadToEnd());
                    Log.Information("ConfigMapper loaded successfully!");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

    }
}
