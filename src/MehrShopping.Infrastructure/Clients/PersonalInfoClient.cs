using MehrShopping.Application.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace MehrShopping.Infrastructure.Clients
{
    public class PersonalInfoClient : IPersonalInfoClient
    {
        private readonly HttpClient _httpClient;

        public PersonalInfoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PersonalInfoDto?> GetAsync(string nationalCode)
        {
            var response = await _httpClient.GetAsync($"api/PersonalInfo/{nationalCode}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var result = await response.Content.ReadFromJsonAsync<PersonalInfoApiResponse>();

            if (result == null)
                return null;

            return new PersonalInfoDto(result.FirstName, result.LastName, result.NationalCode);

        }
    }
}
