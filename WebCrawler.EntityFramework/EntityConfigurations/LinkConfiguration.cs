using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.EntityFramework.Entities;

namespace WebCrawler.EntityFramework.EntityConfigurations
{
    public class LinkConfiguration : IEntityTypeConfiguration<Link>
    {
        public void Configure(EntityTypeBuilder<Link> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Url)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.FromSitemap)
                .IsRequired();

            builder.Property(x => x.FromHtml)
                .IsRequired();

            builder.Property(x => x.ElapsedMilliseconds)
                .IsRequired(false);

            builder.HasOne(x => x.Test)
                .WithMany(z => z.Links)
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
