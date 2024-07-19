using System.Linq;

namespace _Project.Scripts
{
    public static class HelperUtils
    {
        public static string SplitCamelCase(this string str) {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x.ToString() : x.ToString()));
        }
    }
}
