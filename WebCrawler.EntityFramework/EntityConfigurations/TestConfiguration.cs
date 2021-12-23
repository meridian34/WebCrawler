using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.EntityFramework.Entities;

namespace WebCrawler.EntityFramework.EntityConfigurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UserLink).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.TestDateTime).IsRequired();
        }
    }
}
