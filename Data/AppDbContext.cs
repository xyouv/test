using Microsoft.EntityFrameworkCore;

namespace PhoneManagement.Data {
    public class AppDbContext : DbContext {
        IConfiguration _configuration;

        public AppDbContext() {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }

        DbSet<Phone> Phone { get; set; }
        DbSet<User> User { get; set; }
        DbSet<RefreshTokens> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json").Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}
