using ComponentTestDemo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComponentTestDemo.Api.Database
{
    namespace ComponentTestDemo
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            public DbSet<User> Users { get; set; }
        }
    }
}
