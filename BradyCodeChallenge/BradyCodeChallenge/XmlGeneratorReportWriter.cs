using System.Xml;

namespace BradyCodeChallenge
{
    internal class XmlGeneratorReportWriter
    {
        private readonly string filePath;
        private readonly IGeneratorManager generatorManager;

        public XmlGeneratorReportWriter(string filePath, IGeneratorManager generatorManager)
        {
            this.filePath = filePath;
            this.generatorManager = generatorManager;
        }

        public void WriteReport()
        {
            XmlDocument document = new XmlDocument();

            XmlNode generationOutput = document.CreateElement("GenerationOutput");
            XmlElement totals = document.CreateElement("Totals");
            
            foreach (IGenerator generator in this.generatorManager.GetGenerators())
            {
                XmlElement generatorNode = document.CreateElement("Generator");

                XmlElement nameNode = document.CreateElement("Name");
                nameNode.InnerText = generator.Name;

                XmlElement totalNode = document.CreateElement("Total");
                totalNode.InnerText = generator.GetTotalGenerationValue().ToString();

                generatorNode.AppendChild(nameNode);
                generatorNode.AppendChild(totalNode);

                totals.AppendChild(generatorNode);
            }
            generationOutput.AppendChild(totals);

            XmlElement maxEmissionGenerators = document.CreateElement("MaxEmissionGenerators");
            foreach (DateTime day in this.generatorManager.GeneratorOperatingDays)
            {
                if (!generatorManager.TryGetGeneratorWithHighestEmissionsForDate(day, out IGenerator generator))
                {
                    continue;
                }

                XmlElement dayNode = document.CreateElement("Day");

                XmlElement nameNode = document.CreateElement("Name");
                nameNode.InnerText = generator.Name;

                XmlElement dateNode = document.CreateElement("Date");
                dateNode.InnerText = day.ToString();

                XmlElement emissionsNode = document.CreateElement("Emissions");
                generator.TryGetEmissionsForDate(day, out double emissions);
                emissionsNode.InnerText = emissions.ToString();

                dayNode.AppendChild(nameNode);
                dayNode.AppendChild(dateNode);
                dayNode.AppendChild(emissionsNode);

                maxEmissionGenerators.AppendChild(dayNode);
            }
            generationOutput.AppendChild(maxEmissionGenerators);

            XmlElement actualHeatRates = document.CreateElement("ActualHeatRates");
            foreach (ICoalGenerator coalGenerator in generatorManager.GetCoalGenerators())
            {
                XmlElement actualHeatRate = document.CreateElement("ActualHeatRate");

                XmlElement nameNode = document.CreateElement("Name");
                nameNode.InnerText = coalGenerator.Name;

                XmlElement heatRate = document.CreateElement("HeatRate");
                heatRate.InnerText = coalGenerator.GetActualHeatRate().ToString();

                actualHeatRate.AppendChild(nameNode);
                actualHeatRate.AppendChild(heatRate);

                actualHeatRates.AppendChild(actualHeatRate);
            }
            generationOutput.AppendChild(actualHeatRates);

            document.AppendChild(generationOutput);
            document.Save(this.filePath);
        }
    }
}
