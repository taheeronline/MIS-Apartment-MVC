using Microsoft.EntityFrameworkCore;
using ManageApartment.Entities;
using ManageApartment.Repositories.Interface;
using Microsoft.Extensions.Logging;

namespace ManageApartment.Repositories.Implementation
{
    public class UserRepo : iUserRepo
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserRepo> _logger;

        public UserRepo(ApplicationDbContext context, ILogger<UserRepo> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<User> GetUserInfo(string loginName, string password)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(x => x.LoginName.ToLower() == loginName.ToLower() && x.Password == password);

                return user;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting user information.");
                throw;
            }
        }

        public void RegisterUser(User user)
        {
            try
            {
                if (!UserExists(user.LoginName))
                {
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        private bool UserExists(string loginName)
        {
            return _dbContext.Users.Any(x => x.LoginName.ToLower() == loginName.ToLower());
        }
    }
}
