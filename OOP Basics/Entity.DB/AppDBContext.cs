using System;
using Entity.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity.DB
{
    public sealed class AppDbContext : DbContext
    {
        /// <summary>
        /// AppDbContext
        /// </summary>
        /// <param name="options">options</param>
        ///
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<OperationsHistory> OperationsHistory { get; set; }
        public DbSet<TransferHistory> TransferHistory { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        

        /// <inheritdoc />
       
    }
}
