using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatAList.Services.External;

namespace CatAList.Services
{
    /// <summary>
    /// A class that provides methods relating to pet cats and their owners
    /// </summary>
    public class CatService : ICatService
    {
        private IPeopleGetter peopleGetter;

        /// <summary>
        /// Initializes an instance of a cat service
        /// </summary>
        /// <param name="peopleGetter">An <see cref="IPeopleGetter"/> to use to get data to process</param>
        public CatService(IPeopleGetter peopleGetter)
        {
            this.peopleGetter = peopleGetter ?? throw new ArgumentNullException(nameof(peopleGetter));
        }

        /// <summary>
        /// Returns a grouping of cats grouped by the gender of their owners
        /// </summary>
        public async Task<IEnumerable<IGrouping<string, CatNameAndOwnerGender>>> GetCatNamesGroupedByOwnerGendersAsync()
        {
            var people = await peopleGetter.GetAsync();

            if (people == null || people.Count == 0)
                return Enumerable.Empty<IGrouping<string, CatNameAndOwnerGender>>();

            var groups = people
                .Where(o => o.Pets != null)
                .SelectMany(owner => owner.Pets,
                    (owner, pet) => new
                    {
                        OwnerGender = owner.Gender,
                        Name = pet.Name,
                        Type = pet.Type
                    })
                .Where(p => p.Type.Equals("Cat", StringComparison.OrdinalIgnoreCase))
                .Select(p => new CatNameAndOwnerGender
                {
                    Name = p.Name,
                    OwnerGender = p.OwnerGender,
                })
                .OrderBy(p => p.Name)
                .GroupBy(p => p.OwnerGender);

            return groups;
        }
    }
}
