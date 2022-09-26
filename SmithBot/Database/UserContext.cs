using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmithBot.Models;
using System;

namespace SmithBot.Database
{
    public class UserContext : DbContext 
    {
        public UserContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"server=mysql.j81076941.myjino.ru;user=j81076941;password=448agiAoi;database=j81076941_smithbot;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BotUser>()
                .HasMany(u => u.Referrals);
        }
        public DbSet<BotUser> BotUsers { get; set; }
        public DbSet<NewNFT> NFTs { get; set; }

    }
}
