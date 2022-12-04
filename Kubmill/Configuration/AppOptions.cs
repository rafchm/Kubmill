namespace Kubmill.Configuration
{
    /// <summary>
    /// Represents all application options.
    /// </summary>
    public class AppOptions
    {
        public ConfigOptions Config { get; set; }
        public GeneralOptions General { get; set; }

        public AppOptions()
        {
            Config = new ConfigOptions();
            General = new GeneralOptions();
        }

        public static AppOptions Clone(AppOptions options)
        {
            return new AppOptions
            {
                Config = ConfigOptions.Clone(options.Config),
                General = GeneralOptions.Clone(options.General)
            };
        }

        public bool IsSame(AppOptions? options)
        {
            if (options is null)
            {
                return false;
            }

            if (ReferenceEquals(this, options))
            {
                return true;
            }

            return General.Equals(options.General) && Config.Equals(options.Config);
        }
    }
}
