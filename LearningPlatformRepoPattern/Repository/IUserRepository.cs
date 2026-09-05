using LearningPlatformRepoPattern.Models;

namespace LearningPlatformRepoPattern.Repository
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetUserByEmailAndPassword(string email, string password);

        User GetUserById(int userId);

        void UpdateUser(User user);
    }
}
