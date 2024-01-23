namespace BradyCodeChallenge.Generators
{
    internal class WindGenerator : IGenerator
    {
        private readonly ReferenceData referenceData;
        private readonly WindGeneratorLocations location;

        private IList<double> dailyGenerationValues;
        private HashSet<DateTime> daysRunning;

        public WindGenerator(string name, WindGeneratorLocations location, ReferenceData referenceData)
        {
            Name = name;
            this.referenceData = referenceData;
            this.location = location;

            dailyGenerationValues = new List<double>();
            daysRunning = new HashSet<DateTime>();
        }

        public GeneratorType Type
        {
            get
            {
                return GeneratorType.Wind;
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
            double valueFactor;
            switch (this.location)
            {
                case WindGeneratorLocations.Onshore:
                    valueFactor = this.referenceData.ValueFactorData.High;
                    break;
                case WindGeneratorLocations.Offshore:
                    valueFactor = this.referenceData.ValueFactorData.Low;
                    break;
                default: throw new NotSupportedException("Unsopported wind generator location");
            }

            this.dailyGenerationValues.Add(performanceData.Energy * performanceData.Price * valueFactor);
            this.daysRunning.Add(performanceData.Date);
        }

        public bool TryGetEmissionsForDate(DateTime date, out double emissions)
        {
            emissions = 0.0;
            return false;
        }

        public double GetTotalGenerationValue()
        {
            return dailyGenerationValues.Sum();
        }
    }
}
