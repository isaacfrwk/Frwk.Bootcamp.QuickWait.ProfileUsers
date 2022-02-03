using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.AddressContext;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.UserContext;
using Microsoft.EntityFrameworkCore;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new AddressMap());
        }
    }
}
