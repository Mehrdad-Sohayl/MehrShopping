using PersonalInfo.Api.Data;
using PersonalInfo.Api.Domain;

namespace PersonalInfo.Api.Services
{
    public class PersonalInfoService
    {
        private readonly PersonalInfoDbContext _context;

        public PersonalInfoService(PersonalInfoDbContext context)
        {
            _context = context;
        }

        public async Task<Person?> GetAsync(string nationalCode) => await _context.People.FindAsync(nationalCode);
    }
}
