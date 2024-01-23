namespace BradyCodeChallenge
{
    internal interface IGenerator
    {
        string Name { get; }

        GeneratorType Type { get; }

        IReadOnlyCollection<DateTime> DaysRunning { get; }

        void AddGeneratorPerformanceData(GeneratorPerformanceData performanceData);

        double GetTotalGenerationValue();

        bool TryGetEmissionsForDate(DateTime date, out double emissions);
    }
}
