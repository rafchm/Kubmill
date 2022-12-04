using Kubmill.Configuration;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kubmill.Services
{
    /// <inheritdoc/>
    public class ConfigService : IConfigService
    {
        public const string ConfigFileName = "appsettings.json";

        private readonly JsonSerializerOptions _jsonSerOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        private readonly AppOptions _appOptions;

        public ConfigService(IOptions<GeneralOptions> generalOptions, IOptions<ConfigOptions> configOptions)
        {
            _jsonSerOptions.Converters.Add(new JsonStringEnumConverter());
            _appOptions = new AppOptions
            {
                General = generalOptions.Value,
                Config = configOptions.Value
            };
        }

        /// <inheritdoc/>
        public AppOptions GetAppOptions()
        {
            return _appOptions;
        }

        /// <inheritdoc/>
        public void SaveOptions(AppOptions options)
        {
            var outstr = JsonSerializer.Serialize(options, _jsonSerOptions);

            File.WriteAllText(ConfigFileName, outstr);
        }
    }
}
