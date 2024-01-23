using System.Xml;

namespace BradyCodeChallenge
{
    internal class XmlReferenceDataParser
    {
        private string filePath;

        public XmlReferenceDataParser(string filePath)
        {
            this.filePath = filePath;
        }

        public void ParseReferenceData()
        {
            XmlDocument referenceDataDocument = new XmlDocument();
            referenceDataDocument.Load(filePath);

            XmlNode valueFactorNode = GetChildNode(referenceDataDocument, "ReferenceData/Factors/ValueFactor");
            double highValueFactor = double.Parse(GetChildNode(valueFactorNode, "High").InnerText);
            double mediumValueFactor = double.Parse(GetChildNode(valueFactorNode, "Medium").InnerText);
            double lowValueFactor = double.Parse(GetChildNode(valueFactorNode, "Low").InnerText);

            ValueFactorData valueFactorData = new ValueFactorData(highValueFactor, mediumValueFactor, lowValueFactor);

            XmlNode emissionsFactorNode = GetChildNode(referenceDataDocument, "ReferenceData/Factors/EmissionsFactor");
            double highEmissionsFactor = double.Parse(GetChildNode(emissionsFactorNode, "High").InnerText);
            double mediumEmissionsFactor = double.Parse(GetChildNode(emissionsFactorNode, "Medium").InnerText);
            double lowEmissionsFactor = double.Parse(GetChildNode(emissionsFactorNode, "Low").InnerText);

            EmissionsFactorData emissionsFactorData = new EmissionsFactorData(highEmissionsFactor, mediumEmissionsFactor, lowEmissionsFactor);

            ReferenceData.InitialiseReferenceData(valueFactorData, emissionsFactorData);
        }

        private XmlNode GetChildNode(XmlNode parentNode, string childNodeName)
        {
            XmlNode? childNode = parentNode.SelectSingleNode(childNodeName);
            if (childNode == null)
            {
                throw new InvalidDataException($"The node {parentNode.Name} has no child with the name {childNodeName}");
            }

            return childNode;
        }
    }
}
