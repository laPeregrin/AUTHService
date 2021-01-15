using DataBaseStaff.Models;
using Microsoft.EntityFrameworkCore;


namespace DataBaseStaff
{
    public class EfDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(constr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasMany(x => x.Publications).WithOne(x => x.User);
            modelBuilder.Entity<Publication>().HasMany(x => x.HashTags).WithMany(x => x.Publications);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<HashTag> HashaTags { get; set; }

        #region constring
        private const string constr = "Data Source = (localdb)\\MSSQLLocalDB; Database = TestTask; Persist Security Info = false; User ID = 'sa'; Password = 'Ghbdtn010102'; MultipleActiveResultSets = True; Trusted_Connection = False;";
        #endregion
    }
}
