using System;
using Wpf.Ui.Appearance;

namespace Kubmill.Configuration
{
    public class GeneralOptions : IEquatable<GeneralOptions?>
    {
        public ThemeType Theme { get; set; }

        public static GeneralOptions Clone(GeneralOptions options)
        {
            return new GeneralOptions
            {
                Theme = options.Theme
            };
        }

        public bool Equals(GeneralOptions? other)
        {
            return other is not null &&
                   Theme == other.Theme;
        }
    }
}
