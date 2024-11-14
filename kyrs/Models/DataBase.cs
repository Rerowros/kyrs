﻿// File: Models/ApplicationContext.cs
using Microsoft.EntityFrameworkCore;

namespace kyrs.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=Rerowros;database=csharp;",
                new MySqlServerVersion(new Version(8, 0, 39)));
        }
        
        public async Task<User?> ValidateUserAsync(string login, string password)
        {
            return await Users.SingleOrDefaultAsync(u => u.Login == login && u.Password == password);
        }
        
        
        
        
        
        /*Валидация с хешированием*/
        
        /*public async Task<User?> ValidateUserAsync(string login, string password)
        {
            var user = await Users.SingleOrDefaultAsync(u => u.Login == login);
            if (user != null && user.ValidatePassword(password))
            {
                return user;
            }
            return null;
        }*/
    }
    
}