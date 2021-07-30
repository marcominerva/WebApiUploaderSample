using Microsoft.EntityFrameworkCore;
using UploaderSample.DataAccessLayer.Entities;

namespace UploaderSample.DataAccessLayer
{
    public class DataContext : DbContext
    {
        public DbSet<Image> Images { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
