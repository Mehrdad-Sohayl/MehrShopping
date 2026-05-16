namespace MehrShopping.Application.Interfaces
{
    public interface IPersonalInfoService
    {
        Task<PersonalInfoDto> GetAsync(string nationalCode);
    }

    public record PersonalInfoDto(string FirstName, string LastName, string NationalCode);
}
