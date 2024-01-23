namespace BradyCodeChallenge
{
    internal class GeneratorManager : IGeneratorManager
    {
        private List<IGenerator> generators;
        private List<ICoalGenerator> coalGenerators;
        private HashSet<DateTime> daysWithGeneratorsRunning;

        public GeneratorManager()
        {
            this.generators = new List<IGenerator>();
            this.coalGenerators = new List<ICoalGenerator>();
            this.daysWithGeneratorsRunning = new HashSet<DateTime>();
        }

        public IReadOnlyCollection<DateTime> GeneratorOperatingDays
        {
            get
            {
                return this.daysWithGeneratorsRunning;
            }
        }

        public void AddGenerator(IGenerator generator)
        {
            this.generators.Add(generator);
            if (generator.Type == GeneratorType.Coal)
            {
                this.coalGenerators.Add((ICoalGenerator)generator);
            }

            this.daysWithGeneratorsRunning.UnionWith(generator.DaysRunning);
        }

        public IReadOnlyCollection<IGenerator> GetGenerators()
        {
            return this.generators;
        }

        public IReadOnlyCollection<ICoalGenerator> GetCoalGenerators()
        {
            return this.coalGenerators;
        }

        public bool TryGetGeneratorWithHighestEmissionsForDate(DateTime date, out IGenerator generatorWithHighestEmissions)
        {
            double highestEmissions = double.MinValue;
            generatorWithHighestEmissions = null;

            foreach (IGenerator generator in this.generators)
            {
                if (!generator.TryGetEmissionsForDate(date, out double emissions))
                {
                    continue;
                }

                if (emissions > highestEmissions)
                {
                    highestEmissions = emissions;
                    generatorWithHighestEmissions = generator;
                }
            }

            return generatorWithHighestEmissions != null;
        }
    }
}
