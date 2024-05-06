using PhoneManagement.Data;

namespace PhoneManagement.Repository {
    public interface IRefreshTokenRepository {
        int addRefreshTokens(RefreshTokens tokens);
        List<RefreshTokens> getAllRefreshToken();
    }
    public class RefreshTokenRepository : IRefreshTokenRepository {
        public int addRefreshTokens(RefreshTokens tokens) {
            using (var context = new AppDbContext()) {
                var isExist = context.Set<RefreshTokens>().Where(x => x.Id == tokens.Id).FirstOrDefault();
                if (isExist != null) {
                    return 0;
                }
                context.Set<RefreshTokens>().Add(tokens);
                return context.SaveChanges();
            }
        }

        public List<RefreshTokens> getAllRefreshToken() {
            using (var context = new AppDbContext()) {
                return context.Set<RefreshTokens>().ToList();
            }
        }
    }
}
