using BradyCodeChallenge.Generators;

namespace BradyCodeChallenge
{
    internal interface IGeneratorManager
    {
        IReadOnlyCollection<DateTime> GeneratorOperatingDays { get; }

        IReadOnlyCollection<IGenerator> GetGenerators();

        IReadOnlyCollection<ICoalGenerator> GetCoalGenerators();

        bool TryGetGeneratorWithHighestEmissionsForDate(DateTime date, out IGenerator generatorWithHighestEmissions);
    }
}
