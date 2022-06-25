namespace LuccaDevisesIRepositories
{
    public interface IFileTraitmentRepository
    {
        List<string> GetFileLines(string filePath);
        Tuple<string, int, string> ParseConversionLine(string line);
        Tuple<string, string, double> ParseExchangeRate(int lineNumber, string line);
        List<Tuple<string, string, double>> ParseExchangeRates(List<Tuple<int, string>> lines);
    }
}