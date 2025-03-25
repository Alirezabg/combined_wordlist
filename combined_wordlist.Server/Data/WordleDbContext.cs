using combined_wordlist.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace combined_wordlist.Server.Data
{
    public class WordleDbContext : DbContext
    {
        public DbSet<WordEntity> Words { get; set; }
        public WordleDbContext(DbContextOptions<WordleDbContext> options) : base(options) { }

    }
}
