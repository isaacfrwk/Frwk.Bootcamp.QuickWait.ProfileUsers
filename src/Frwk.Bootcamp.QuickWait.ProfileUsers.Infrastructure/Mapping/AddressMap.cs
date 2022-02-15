using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.AddressContext
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.UserId);
        }
    }
}
