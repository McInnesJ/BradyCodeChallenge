namespace BradyCodeChallenge
{
    internal struct EmissionsFactorData
    {
        public EmissionsFactorData(double high, double medium, double low)
        {
            this.High = high;
            this.Medium = medium;
            this.Low = low;
        }

        public double High { get; }
        public double Medium { get; }
        public double Low { get; }
    }
}
