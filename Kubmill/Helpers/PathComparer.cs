using System;

namespace Kubmill.Helpers
{
    public static class PathComparer
    {
        public static bool Equals(string? path1, string? path2)
        {
            var isCaseSensitive = Environment.OSVersion.Platform == PlatformID.Unix;

            return string.Compare(path1 ?? "", path2 ?? "", isCaseSensitive) == 0;
        }
    }
}
