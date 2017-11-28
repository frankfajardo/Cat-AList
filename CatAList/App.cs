using CatAList.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CatAList
{
    public class App
    {
        private ICatService catService;
        private TextWriter outputWriter;
        private TextReader inputReader;

        public const string NoInfoText = "No info to return.";
        public const string PetNamePointer = "  * ";
        public const string PressAnyKeyToContinue = "\nPress any key to continue.";

        public App(ICatService catService, TextWriter outputWriter, TextReader inputReader)
        {
            this.catService = catService ?? throw new ArgumentNullException(nameof(catService));
            this.outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
            this.inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
        }

        public async Task RunAsync()
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

            outputWriter.WriteLine(PressAnyKeyToContinue);
            inputReader.Read();
        }
    }
}
