using SimpleFund.Domain.Utils;

namespace SimpleFund.Infrastructure.Utils
{
    public class ParseUtil : IParseUtil
    {
        public double? ToDoubleNullable(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return null;
            }

            double value;

            if (double.TryParse(target, out value))
            {
                return value;
            }

            return null;
        }
    }
}
