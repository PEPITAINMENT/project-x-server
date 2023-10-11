namespace GameBussinesLogic.Comparer.FuzzyComaprer
{
    public interface IFuzzyComparer
    {
        bool IsMatch(string main, string matching);
    }
}
