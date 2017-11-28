using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatAList.Services.External.Models;

namespace CatAList.Services.External
{
    /// <summary>
    /// Represents a helper for getting data from a People (Web) Service
    /// </summary>
    public interface IPeopleGetter
    {
        /// <summary>
        /// Gets the data from the service
        /// </summary>
        /// <returns></returns>
        Task<List<PetOwner>> GetAsync();
    }
}
