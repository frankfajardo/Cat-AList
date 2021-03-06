﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatAList.Services
{
    /// <summary>
    /// Represents a service that provides methods relating to pet cats and their owners
    /// </summary>
    public interface ICatService
    {
        /// <summary>
        /// Returns a grouping of cats grouped by the gender of their owners
        /// </summary>
        Task<IEnumerable<IGrouping<string, CatNameAndOwnerGender>>> GetCatNamesGroupedByOwnerGendersAsync();
    }
}
