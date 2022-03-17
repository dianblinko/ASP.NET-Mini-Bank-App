namespace Minibank.Core
{
    public interface IExchangeRateSource
    {
        double GetValuteCourse(string code);
    }
}
