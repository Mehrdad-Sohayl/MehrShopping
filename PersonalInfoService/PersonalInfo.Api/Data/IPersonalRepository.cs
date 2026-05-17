using PersonalInfo.Api.Domain;

namespace PersonalInfo.Api.Data
{
    public interface IPersonalRepository
    {
        Task<Person?> FindByNationalCodeAsync(string nationalCode);
    }
}
