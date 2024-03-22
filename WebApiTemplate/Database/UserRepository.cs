using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Database;

namespace WebApiTemplate.Data
{
    public class UserRepository
    {
        private readonly UserContext _appDbContext;

        public UserRepository(UserContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddEmployeeAsync(User1 user)
        {
            await _appDbContext.Set<User1>().AddAsync(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<User1>> GetAllEmployeeAsync()
        {
            return await _appDbContext.User1.ToListAsync();
        }

        public async Task<User1> GetEmployeeByIdAsync(int id)
        {
            return await _appDbContext.User1.FindAsync(id);
        }

        public async Task UpdateEmployeeAsync(int id, User1 model)
        {
            var employeee = await _appDbContext.User1.FindAsync(id);
            if (employeee == null)
            {
                throw new Exception("Employee not found");
            }
            employeee.Id_Card = model.Id_Card;
            employeee.Username = model.Username;
            employeee.Password = model.Password;
            employeee.Role = model.Role;
            employeee.Names = model.Names;
            employeee.Phone = model.Phone;
            employeee.Email = model.Email;
            employeee.City = model.City;
            employeee.Registration_date = model.Registration_date;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsnyc(int id)
        {
            var employeee = await _appDbContext.User1.FindAsync(id);
            if (employeee == null)
            {
                throw new Exception("User not found");
            }
            _appDbContext.User1.Remove(employeee);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<User1> GetEmplaoyeeByUsernamel(string username)
        {
            return await _appDbContext.User1.Where(x => x.Username == username).FirstOrDefaultAsync();
        }
    }
}
