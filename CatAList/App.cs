using CatAList.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CatAList
{
    public class App
    {
        private ILogger<App> logger;
        private ICatService catService;
        private TextWriter outputWriter;
        private TextReader inputReader;

        public const string NoInfoText = "No info to return.";
        public const string PetNamePointer = "  * ";
        public const string PressAnyKeyToContinue = "\nPress any key to continue.";
        public const string UnexpectedErrorText = "Oops, something went wrong whilst trying to get data to process. I'm unable to give you the results.";

        public App(ILogger<App> logger, ICatService catService, TextWriter outputWriter, TextReader inputReader)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.catService = catService ?? throw new ArgumentNullException(nameof(catService));
            this.outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
            this.inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
        }

        public async Task RunAsync()
        {
            try
            {
                var catGroups = await catService.GetCatNamesGroupedByOwnerGendersAsync();

                if (catGroups == null || catGroups.Count() == 0)
                    outputWriter.WriteLine(NoInfoText);
                else
                    foreach (var catGroup in catGroups)
                    {
                        outputWriter.WriteLine(catGroup.FirstOrDefault().OwnerGender);
                        foreach (var cat in catGroup)
                        {
                            outputWriter.WriteLine($"{PetNamePointer}{cat.Name}");
                        }
                        outputWriter.WriteLine();
                    }
            }
            catch (Exception e)
            {
                logger.LogError("App received " + e.ToString() + ": " + e.Message + " Call stack: " + e.StackTrace);
                outputWriter.WriteLine(UnexpectedErrorText);
                outputWriter.WriteLine();
            }

            outputWriter.WriteLine(PressAnyKeyToContinue);
            inputReader.Read();
        }
    }
}
