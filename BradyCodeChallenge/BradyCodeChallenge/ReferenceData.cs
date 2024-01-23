namespace BradyCodeChallenge
{
    internal class ReferenceData
    {
        private ReferenceData (ValueFactorData valueFactorData, EmissionsFactorData emissionsFactorData)
        {
            this.ValueFactorData = valueFactorData;
            this.EmissionsFactorData = emissionsFactorData;
        }

        public static void InitialiseReferenceData(ValueFactorData valueFactorData, EmissionsFactorData emissionsFactorData)
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("Reference Data already initialised");
            }

            Instance = new ReferenceData(valueFactorData, emissionsFactorData);
        }

        // Handle getter when not yet initialised
        public static ReferenceData Instance { get; private set; }
        
        public ValueFactorData ValueFactorData { get; }
        public EmissionsFactorData EmissionsFactorData { get; }

    }
}
