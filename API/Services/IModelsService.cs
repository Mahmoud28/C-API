using API.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IModelsService
    {
        Response GetModelsForMakeIdYear(int makeId, int modelyear);
    }
    public class ModelsService : IModelsService
    {
        private readonly IConfiguration configuration;
        public ModelsService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Response GetModelsForMakeIdYear(int makeId, int modelyear)
        {
            string url = string.Format(configuration.GetSection("GetModelsForMakeIdYearAPIURL").Get<string>(), makeId, modelyear);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                return response.Content.ReadAsAsync<Response>().Result;  
            }
            else
            {
                return null;
            }
            
        }
    }
}
