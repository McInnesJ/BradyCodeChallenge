Overview:
1. Read in data from the reference file. This information cannot change once the program is running so implement as a singleton to ensure only a single instance is created at a time.
2. I Introduced a 'GeneratorManager' This class is responsible for storing the individual generators. It also stores a list of days for which any generator is running. Storing this information as data is being read allows for much simpler generation of the output
3. I then introduced IGenerators. These are responsible for storing all the relevant information per generator. ICoalGenerator extends IGenerator, as coal generators have unique behaviour around the ActualHeatRate values.
4. The XmlGeneratorDataParser parsers the file provided, creating generators, populating the daily generation information, and adding these generators to the Generator Manager.
5. The XmlGeneratorReportWriter takes a Generator Manager and constructs the relevant output xml file.

Points to note:
1. Each generator keeps track of the days it has been operating for in a hashset. The generator manager tracks the days for which any generator has been operating by performing the union of the current days, and the days of the generator being added.
   When building the highest daily emissions part of the report the generator manager is queried, and this is responsible for identifying the generator with the highest emissions. Each generator has a TryGet method for emissions for that day. Overall this approach should ensure that
   accurate reports can still be built even if some generators are unoperational on some days.

What I would work on next
1. Most importantly, I ran out of time to write unit tests, so this would be my first priority. I have designed the solution with testing in mind (using Dependancy Injections and trying to keep coupling low) and so adding tests should be straight forwards.
2. I did not get a chance to implement the file watching functionality, though my plan was FileSystemWatcher, and create an event handler for the OnCreated event. I would have added a new class responsible for orchestrating the read and write operations. This class would have had a 'GenerateReports' method, and the event handler
   for 'OnCreate' would have called GenerateReports. It's a shame I ran out of time for this because it is not something I'd done before and it seems interesting. Hopefully I'll find some time to give it a go before any potential interview!
