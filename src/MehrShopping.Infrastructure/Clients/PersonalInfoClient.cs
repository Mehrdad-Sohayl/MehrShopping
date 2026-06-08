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
            if (_httpClient.Timeout == Timeout.InfiniteTimeSpan)
                _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public async Task<PersonalInfoDto?> GetAsync(string nationalCode, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"api/PersonalInfo/{nationalCode}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var result = await response.Content.ReadFromJsonAsync<PersonalInfoApiResponse>(cancellationToken: cancellationToken);

            if (result == null)
                return null;

            return new PersonalInfoDto(result.FirstName, result.LastName, result.NationalCode);
        }
    }
}
