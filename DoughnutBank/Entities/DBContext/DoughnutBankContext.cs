using Microsoft.EntityFrameworkCore;

namespace DoughnutBank.Entities.DBContext
{
    public class DoughnutBankContext : DbContext 

    {
        public DoughnutBankContext(DbContextOptions<DoughnutBankContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<OTP> OTPs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email);
                entity.HasOne(user => user.OTP);
            });

            modelBuilder.Entity<OTP>(entity =>
            {
                entity.HasKey(otp => otp.UserEmail);
            });
        }
    }
}
