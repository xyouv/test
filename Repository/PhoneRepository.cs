using Microsoft.EntityFrameworkCore;
using PhoneManagement.Data;

namespace PhoneManagement.Repository {

    public interface IPhoneRepository {
        List<Phone> getAllPhones();
        int addNewPhone(Phone phone);
        Phone? getPhoneById(int id);
        int updatePhone(Phone phone, int id);
        int deletePhoneById(int id);
    }
    public class PhoneRepository : IPhoneRepository {
        //private DbSet<Phone> _DbSet;

        public PhoneRepository() {
        }

        public List<Phone> getAllPhones() {
            using (var context = new AppDbContext()) {
                var cars = context.Set<Phone>().AsNoTracking().ToList();
                return cars;
            }
        }

        public int addNewPhone(Phone phone) {
            using (var context = new AppDbContext()) {
                var isExist = context.Set<User>().AsNoTracking().Where(x => x.Id == phone.UserId).FirstOrDefault();
                if (isExist == null) {
                    return 0;
                }
                context.Set<Phone>().Add(phone);
                return context.SaveChanges();
            }
        }

        public Phone? getPhoneById(int id) {
            using (var context = new AppDbContext()) {
                var isExist = context.Set<Phone>().AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
                return isExist;
            }
        }

        public int updatePhone(Phone phone, int id) {
            using (var context = new AppDbContext()) {
                var isExist = getPhoneById(id);
                if (isExist == null) {
                    return 0;
                }
                context.Set<Phone>().Update(phone);
                return context.SaveChanges();
            }
        }

        public int deletePhoneById(int id) {
            using(var context = new AppDbContext()) {
                var isExist = getPhoneById(id);
                if (isExist == null) {
                    return 0;
                }
                context.Set<Phone>().Remove(isExist);
                return context.SaveChanges();
            }
        }
    }
}
