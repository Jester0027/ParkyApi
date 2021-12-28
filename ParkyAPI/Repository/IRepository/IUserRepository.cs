using ParkyAPI.Models;

namespace ParkyAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUserUnique(string username);
        User Authenticate(string username, string password);
        User Register(string username, string password);
    }
}