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
            optionsBuilder.UseSqlite("Filename=Db/Clients.db");
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
