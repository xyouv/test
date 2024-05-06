using PhoneManagement.Data;

namespace PhoneManagement.Repository {

    public interface IUserRepository {
        User? getUser(string username, string password);
        User? getUserByUserID(int userId);
        User? getUserByAccNEmail(string username, string email);
        int addUser(User user);
        List<User> getAllUsers();
        int updateVerifyUser(User isExist);
    }
    public class UserRepository : IUserRepository {
        public UserRepository() { }

        public int addUser(User user) {
            using (var context = new AppDbContext()) {
                var isExist = getUserByAccNEmail(user.Account, user.Email);
                if (isExist != null) {
                    return 0;
                }
                context.Set<User>().Add(user);
                return context.SaveChanges();
            }
        }

        public List<User> getAllUsers() {
            using (var context = new AppDbContext()) {
                return context.Set<User>().ToList();
            }
        }

        public User? getUser(string username, string password) {
            using (var context = new AppDbContext()) {
                var user = context.Set<User>().Where(x => x.Account == username && x.Password == password).FirstOrDefault();
                return user;
            }
        }

        public User? getUserByAccNEmail(string username, string email) {
            using (var context = new AppDbContext()) {
                var user = context.Set<User>().Where(x => x.Email == email || x.Account == username).FirstOrDefault();
                return user;
            }
        }

        public User? getUserByUserID(int userId) {
            using (var context = new AppDbContext()) {
                var user = context.Set<User>().Where(x => x.Id == userId).FirstOrDefault();
                return user;
            }
        }

        public int updateVerifyUser(User user) {
            using (var context = new AppDbContext()) {
                context.Set<User>().Update(user);
                return context.SaveChanges();
            }
        }
    }
}
