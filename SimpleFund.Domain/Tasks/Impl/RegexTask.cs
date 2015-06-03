using System.Text.RegularExpressions;

namespace SimpleFund.Domain.Tasks.Impl
{
    public class RegexTask : IRegexTask
    {
        private static readonly Regex NumberRegex = new Regex(@"(-?\d+)(\.\d+)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex UnitRegex = new Regex(@"[^\d\.].*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public string MatchNumber(string target)
        {
            return NumberRegex.Match(target).Value;
        }

        public string MatchUnit(string target)
        {
            return UnitRegex.Match(target).Value;
        }
    }
}
