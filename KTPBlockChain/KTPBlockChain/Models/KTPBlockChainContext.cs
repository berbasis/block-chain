using Microsoft.EntityFrameworkCore;

namespace KTPBlockChain.Models
{
    public class KTPBlockChainContext : DbContext
    {
        public KTPBlockChainContext(DbContextOptions<KTPBlockChainContext> options)
            : base(options)
        {
        }

        public DbSet<Block> Block { get; set; }
        public DbSet<KTP> KTP { get; set; }
        public DbSet<Pool> Pool { get; set; }
    }
}