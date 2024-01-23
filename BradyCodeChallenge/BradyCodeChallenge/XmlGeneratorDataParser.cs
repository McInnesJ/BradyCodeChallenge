using System.Xml;
using BradyCodeChallenge.Generators;

namespace BradyCodeChallenge
{
    internal class XmlGeneratorDataParser
    {
        private readonly string filePath;
        private readonly ReferenceData referenceData;

        public XmlGeneratorDataParser(string filePath, ReferenceData referenceData)
        {
            this.filePath = filePath;
            this.referenceData = referenceData;
        }

        public IGeneratorManager ParseGeneratorData()
        {
            GeneratorManager generatorManager = new GeneratorManager();

            XmlDocument generatorDataDocument = new XmlDocument();
            generatorDataDocument.Load(filePath);

            XmlNode? rootNode = generatorDataDocument.SelectSingleNode("GenerationReport");
            if (rootNode ==  null )
            {
                throw new InvalidDataException($"No generator data found in file {this.filePath}");
            }
            
            XmlNodeList childNodes = rootNode.ChildNodes;

            foreach (XmlNode generatorTypeNode in childNodes)
            {
                if (!Enum.TryParse(generatorTypeNode.Name, out GeneratorType generatorType))
                {
                    // TODO: Error handling probably does not need to be this strict, it is likely we can continue to
                    // generate the report and add a note stating some data is missing
                    throw new NotSupportedException("Report contains an unsupported generator type");
                }

                XmlNodeList generatorNodes = generatorTypeNode.ChildNodes;

                switch (generatorType)
                {
                    case GeneratorType.Wind:
                        ParseWindGenerators(generatorManager, generatorNodes);
                        break;
                    case GeneratorType.Gas:
                        ParseGasGenerators(generatorManager, generatorNodes);
                        break;
                    case GeneratorType.Coal:
                        ParseCoalGenerators(generatorManager, generatorNodes);
                        break;
                    default: throw new NotSupportedException("Unsupported generator type");
                }
            }
            return generatorManager;
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

        private string GetGeneratorName(XmlNode generator)
        {
            XmlNode nameNode = GetChildNode(generator, "Name");
            string name = nameNode.InnerText;

            return name;
        }

        private GeneratorPerformanceData GetGeneratorPerformanceData(XmlNode day)
        {
            string dateTimeString = GetChildNode(day, "Date").InnerText;
            string energyString = GetChildNode(day, "Energy").InnerText;
            string priceString = GetChildNode(day, "Price").InnerText;

            if (dateTimeString == null || energyString == null || priceString == null)
            {
                throw new InvalidDataException($"Report data file '{this.filePath}' contains incomplete generator data");
            }

            DateTime date = DateTime.Parse(dateTimeString);
            double energy = double.Parse(energyString);
            double price = double.Parse(priceString);

            return new GeneratorPerformanceData(date, energy, price);
        }

        private void ParseDailyGenerationData(XmlNode generatorNode, IGenerator generator)
        {
            XmlNodeList dayNodes = GetChildNode(generatorNode, "Generation").ChildNodes;
            foreach (XmlNode dayNode in dayNodes)
            {
                generator.AddGeneratorPerformanceData(GetGeneratorPerformanceData(dayNode));
            }
        }

        private void ParseWindGenerators(GeneratorManager generatorManager, XmlNodeList generatorNodes)
        {
            foreach (XmlNode generatorNode in generatorNodes)
            {
                string generatorName = GetGeneratorName(generatorNode);
                string locationString = GetChildNode(generatorNode, "Location").InnerText;

                if (!Enum.TryParse(locationString, out WindGeneratorLocations location))
                {
                    throw new InvalidDataException($"Unsupported location type {locationString}");
                }

                IGenerator generator = new WindGenerator(generatorName, location, this.referenceData);
                ParseDailyGenerationData(generatorNode, generator);
                
                generatorManager.AddGenerator(generator);
            }
        }

        private void ParseGasGenerators(GeneratorManager generatorManager, XmlNodeList generatorNodes)
        {
            foreach (XmlNode generatorNode in generatorNodes)
            {
                string generatorName = GetGeneratorName(generatorNode);
                string emissionsRatingString = GetChildNode(generatorNode, "EmissionsRating").InnerText;
                double emissionsRating = double.Parse(emissionsRatingString);

                IGenerator generator = new GasGenerator(generatorName, emissionsRating, this.referenceData);
                ParseDailyGenerationData(generatorNode, generator);

                generatorManager.AddGenerator(generator);
            }
        }

        private void ParseCoalGenerators(GeneratorManager generatorManager, XmlNodeList generatorNodes)
        {
            foreach (XmlNode generatorNode in generatorNodes)
            {
                string generatorName = GetGeneratorName(generatorNode);

                string totalHeatInputString = GetChildNode(generatorNode, "TotalHeatInput").InnerText;
                string actualNetGenerationString = GetChildNode(generatorNode, "ActualNetGeneration").InnerText;
                string emissionsRatingString = GetChildNode(generatorNode, "EmissionsRating").InnerText;
                
                double totalHeatRating = double.Parse(totalHeatInputString);
                double actualNetGeneration = double.Parse(actualNetGenerationString);
                double emissionsRating = double.Parse(emissionsRatingString);


                IGenerator generator = new CoalGenerator(generatorName, emissionsRating, this.referenceData, totalHeatRating, actualNetGeneration);
                ParseDailyGenerationData(generatorNode, generator);

                generatorManager.AddGenerator(generator);
            }
        }
    }
}
