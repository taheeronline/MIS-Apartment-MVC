using ManageApartment.Entities;

namespace ManageApartment.Repositories.Interface
{
    public interface iUserRepo
    {
        void RegisterUser(User user);

        Task<User> GetUserInfo(string loginName, string password);

    }
}
