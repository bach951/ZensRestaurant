using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Models;
using static System.Formats.Asn1.AsnWriter;

namespace Repository.DBContext
{
    public class ZRDbContext : DbContext
    {
        public ZRDbContext()
        {

        }
        public ZRDbContext(DbContextOptions<ZRDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                  .SetBasePath(Directory.GetCurrentDirectory())
                                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDbStore"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User
            modelBuilder.Entity<User>(user =>
            {
                user.Property(prop => prop.UserName).IsUnicode(false).HasMaxLength(100).IsRequired(true);
                user.Property(prop => prop.Email).IsUnicode(false).HasMaxLength(100).IsRequired(true);
                user.Property(prop => prop.Password).IsUnicode(false).HasMaxLength(50).IsRequired(true);
                user.Property(prop => prop.Address).IsUnicode(false).HasMaxLength(200).IsRequired(false);
                user.Property(prop => prop.Status).IsRequired(true);
                user.Property(prop => prop.DOB).HasColumnType("datetime2").IsRequired(true);
                user.Property(prop => prop.Gender).IsUnicode(false).IsRequired(true);
                user.Property(prop => prop.Role).IsUnicode(false).HasMaxLength(20).IsRequired(true);
                user.Property(prop => prop.Avatar).IsUnicode(false).HasMaxLength(int.MaxValue).IsRequired(false);
            });
            #endregion

            #region RefreshToken
            modelBuilder.Entity<RefreshToken>(rft =>
            {
                rft.Property(prop => prop.Token).IsUnicode(false).HasMaxLength(int.MaxValue).IsRequired(true);
                rft.Property(prop => prop.ExpiredDate).HasColumnType("datetime2").IsRequired(true);
                rft.Property(prop => prop.JWTId).IsUnicode(false).HasMaxLength(int.MaxValue).IsRequired(true);
            });

            modelBuilder.Entity<RefreshToken>()
             .HasOne(r => r.User)
             .WithMany(user => user.RefreshTokens);
            #endregion
        }
    }
}
