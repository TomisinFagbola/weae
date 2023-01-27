using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Domain.Entities;

namespace Infrastructure.Data.DbContext.Configurations
{
    public class WeatherConfiguration : IEntityTypeConfiguration<Weather>
    {
        public void Configure(EntityTypeBuilder<Weather> builder)
        {
            builder.Property(x => x.LowTemparature).IsRequired();
            builder.Property(x => x.HighTemperature).IsRequired();
            builder.Property(x => x.Pressure).IsRequired();
            builder.Property(x => x.Humidity).IsRequired();
            builder.Property(x => x.Type).IsRequired();


            builder.HasOne(x => x.State)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
