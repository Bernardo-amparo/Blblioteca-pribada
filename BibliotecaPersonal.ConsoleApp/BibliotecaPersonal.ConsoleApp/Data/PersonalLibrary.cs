using BibliotecaPersonal.ConsoleApp.Moduls;
using Microsoft.EntityFrameworkCore;
using System;

namespace BibliotecaPersonal.ConsoleApp.Data
{
    public class PersonalLibrary
    {
        public class LibraryContext : DbContext
        {
            public DbSet<Book> Books { get; set; }
            public DbSet<Author> Authors { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Person> People { get; set; }
            public DbSet<Loan> Loans { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-6TH38FO1\\SQLEXPRESS;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}