using PersonalInfo.Api.Domain;

namespace PersonalInfo.Api.Data
{
    public class PersonalRepository : IPersonalRepository
    {
        private readonly PersonalInfoDbContext _context;

        public PersonalRepository(PersonalInfoDbContext context)
        {
            _context = context;
        }

        public async Task<Person?> FindByNationalCodeAsync(string nationalCode)
        {
            return await _context.People.FindAsync(nationalCode);
        }
    }
}
