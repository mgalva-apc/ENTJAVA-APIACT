using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class CharasDbContext : DbContext
    {
        public CharasDbContext(DbContextOptions<CharasDbContext> options)
            : base(options)
        {
        }

        public DbSet<CharasEntity> CharaItems { get; set; } = null!;
    }
}
