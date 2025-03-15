using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Short_URL_INFORCE.Models;
using System;

namespace Short_URL_INFORCE.Data
{
    public class AppDBContext : IdentityDbContext<IdentityUser>

    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<URL> URLs { get; set; }
        public DbSet<About> About { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<URL>()
                .HasIndex(u => u.FullUrl)
                .IsUnique();
        }
    }
}

