using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebCrawler.EntityFramework
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-5NI0SMB; Initial Catalog=WebCrawlerDB; Integrated Security=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
