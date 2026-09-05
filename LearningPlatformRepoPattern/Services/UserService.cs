using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LearningPlatformRepoPattern.Services
{

    public class UserService : IUserRepository
    {
        private readonly ApplicationDbContext db;
        public UserService(ApplicationDbContext db)
        {
            this.db = db;
            
        }
        public void AddUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return db.Users.FirstOrDefault(u =>
             u.UserEmail == email &&
             u.UserPassword == password);
        }

        public User GetUserById(int userId)
        {
            return db.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public void UpdateUser(User user)
        {
            db.Users.Update(user);
            db.SaveChanges();
        }
    }
}
