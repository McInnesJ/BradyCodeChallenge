namespace BradyCodeChallenge
{
    internal struct ValueFactorData
    {
        public ValueFactorData(double high, double medium, double low)
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
