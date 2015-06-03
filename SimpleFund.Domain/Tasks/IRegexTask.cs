namespace SimpleFund.Domain.Tasks
{
    public interface IRegexTask
    {
        string MatchNumber(string target);
        string MatchUnit(string target);
    }
}
