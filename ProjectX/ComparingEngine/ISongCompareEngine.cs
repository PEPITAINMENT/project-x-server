namespace ComparingEngine
{
    public interface ISongCompareEngine
    {
        GuessSongProperties Compare(string userGuess, ISong song);
    }
}
