using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.DbContext.Configurations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(125);


            builder.HasOne(x => x.Country)
              .WithMany()
              .HasForeignKey(x => x.CountryId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
