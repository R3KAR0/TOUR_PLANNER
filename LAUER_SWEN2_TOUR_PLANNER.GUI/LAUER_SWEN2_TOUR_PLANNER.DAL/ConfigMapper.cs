using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.DAL
{
    public class ConfigMapper
    {
        public enum ConfigMode { TESTING, PRODUCTION}


        [JsonPropertyName("connectionString")]
        public string ConnectionString { get; private set; }
        [JsonPropertyName("username")]
        public string DBUser { get; private set; }
        [JsonPropertyName("password")]
        public string DBPassword { get; private set; }

        [JsonPropertyName("postgresDoubleEntryCode")]
        public string PostgresDoubleEntry { get; private set; }

        [JsonPropertyName("mapQuestKey")]
        public string MapQuestKey { get; private set; }

        [JsonPropertyName("mapQuestSecret")]
        public string MapQuestSecret { get; private set; }

        [JsonPropertyName("secret")]
        public string Secret { get; private set; }

        [JsonPropertyName("export")]
        public string Export { get; private set; }

        [JsonPropertyName("import")]
        public string Import { get; private set; }




        private static ConfigMapper? configMapper;

        public ConfigMapper(string connectionString, string dBUser, string dBPassword, string postgresDoubleEntry, string mapQuestKey, string mapQuestSecret, string secret, string export, string import)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            DBUser = dBUser ?? throw new ArgumentNullException(nameof(dBUser));
            DBPassword = dBPassword ?? throw new ArgumentNullException(nameof(dBPassword));
            PostgresDoubleEntry = postgresDoubleEntry ?? throw new ArgumentNullException(nameof(postgresDoubleEntry));
            MapQuestKey = mapQuestKey ?? throw new ArgumentNullException(nameof(mapQuestKey));
            MapQuestSecret = mapQuestSecret ?? throw new ArgumentNullException(nameof(mapQuestSecret));
            Secret = secret ?? throw new ArgumentNullException(nameof(secret));
            Export = export ?? throw new ArgumentNullException(nameof(export));
            Import = import ?? throw new ArgumentNullException(nameof(import));
        }

        public static ConfigMapper? GetConfigMapper(ConfigMode mode = ConfigMode.PRODUCTION)
        {
            if (configMapper == null)
            {
                if(mode == ConfigMode.TESTING)
                {
                    SetupMapper("..\\..\\test_config.json");
                }
                else
                {
                    SetupMapper();
                }
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
