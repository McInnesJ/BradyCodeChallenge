namespace BradyCodeChallenge
{
    internal class GasGenerator : IGenerator
    {
        private readonly double valueFactor;
        private readonly double emissionFactor;
        private readonly double emissionRating;

        private IList<double> dailyGenerationValues;
        private IDictionary<DateTime, double> dailyEmissions;
        private HashSet<DateTime> daysRunning;

        public GasGenerator(string name, double emissionRating, ReferenceData referenceData)
        {
            Name = name;
            this.valueFactor = referenceData.ValueFactorData.Medium;
            this.emissionFactor = referenceData.EmissionsFactorData.Medium;
            this.emissionRating = emissionRating;

            this.dailyGenerationValues = new List<double>();
            this.dailyEmissions = new Dictionary<DateTime, double>();
            this.daysRunning = new HashSet<DateTime>();
        }

        public GeneratorType Type
        {
            get
            {
                return GeneratorType.Gas;
            }
        }

        public string Name { get; }

        public IReadOnlyCollection<DateTime> DaysRunning
        {
            get
            {
                return this.daysRunning;
            }
        }

        public void AddGeneratorPerformanceData(GeneratorPerformanceData performanceData)
        {
            double energy = performanceData.Energy;

            double dailyGenerationValue = energy * performanceData.Price * this.valueFactor;
            this.dailyGenerationValues.Add(dailyGenerationValue);

            double dailyEmission = energy * this.emissionRating * this.emissionFactor;
            this.dailyEmissions.Add(performanceData.Date.Date, dailyEmission);

            this.daysRunning.Add(performanceData.Date);
        }

        public bool TryGetEmissionsForDate(DateTime date, out double emissions)
        {
            return this.dailyEmissions.TryGetValue(date, out emissions);

        }

        public double GetTotalGenerationValue()
        {
            return this.dailyGenerationValues.Sum();
        }
    }
}
