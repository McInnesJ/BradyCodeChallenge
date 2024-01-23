namespace BradyCodeChallenge
{
    internal readonly struct GeneratorPerformanceData
    {
        public GeneratorPerformanceData(DateTime date, double energy, double price)
        {
            Date = date;
            Energy = energy;
            Price = price;
        }

        public DateTime Date { get; }

        public double Energy { get; }

        public double Price { get; }
    }
}
