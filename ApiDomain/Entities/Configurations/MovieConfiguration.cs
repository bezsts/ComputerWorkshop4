using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiDomain.Entities.Configurations
{
    internal class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(m => m.Director)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(m => m.Genre)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(m => m.IsRealeased)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(m => m.ReleaseDate)
                .HasDefaultValue(DateOnly.FromDateTime(DateTime.Now))
                .IsRequired();
        }
    }
}
