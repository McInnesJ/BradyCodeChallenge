using BradyCodeChallenge;

XmlReferenceDataParser xmlReferenceDataParser = new XmlReferenceDataParser(@"C:\temp\ReferenceData.xml");
xmlReferenceDataParser.ParseReferenceData();

XmlGeneratorDataParser xmlGeneratorDataParser = new XmlGeneratorDataParser(@"C:\temp\01-Basic.xml", ReferenceData.Instance);
IGeneratorManager generatorManager = xmlGeneratorDataParser.ParseGeneratorData();

XmlGeneratorReportWriter xmlGeneratorReportWriter = new XmlGeneratorReportWriter(@"C:\temp\outputFile.xml", generatorManager);
xmlGeneratorReportWriter.WriteReport();