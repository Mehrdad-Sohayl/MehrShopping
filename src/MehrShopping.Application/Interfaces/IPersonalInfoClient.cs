namespace MehrShopping.Application.Interfaces
{
    public interface IPersonalInfoClient
    {
        Task<PersonalInfoDto?> GetAsync(string nationalCode, CancellationToken cancellationToken);
    }

    public record PersonalInfoDto(string FirstName, string LastName, string NationalCode);
}
