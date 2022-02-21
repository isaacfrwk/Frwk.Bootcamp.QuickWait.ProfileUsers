using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.Data.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options){ }
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Address> Addresses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new AddressMap());
        }

    }
}
