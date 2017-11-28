using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CatAList.Services.External.Models;

namespace CatAList.Services.External
{
    /// <summary>
    /// A class for getting data from a People (Web) Service
    /// </summary>
    public class PeopleGetter : IPeopleGetter
    {
        private ILogger<PeopleGetter> logger;
        private static readonly HttpClient httpClient = new HttpClient();
        private PeopleServiceInfo peopleServiceInfo;

        public PeopleGetter(ILogger<PeopleGetter> logger,
            PeopleServiceInfo peopleServiceInfo)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.peopleServiceInfo = peopleServiceInfo ?? throw new ArgumentNullException(nameof(peopleServiceInfo));
            httpClient.BaseAddress = new Uri(peopleServiceInfo.BaseUrl);
        }

        /// <summary>
        /// Gets the pet owner list from the people service
        /// </summary>
        /// <returns></returns>
        public async Task<List<PetOwner>> GetAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync(this.peopleServiceInfo.EndPoint);
                List<PetOwner> items = JsonConvert.DeserializeObject<List<PetOwner>>(json);
                return items;
            }
            catch
            {
                logger.LogError("Failed to get from external People Web Service");
                throw;
            }
        }
    }
}
