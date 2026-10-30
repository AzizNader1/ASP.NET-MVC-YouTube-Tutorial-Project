using FirstMVCProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstMVCProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, BookName = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = Genres.Fiction },
                new Book { Id = 2, BookName = "To Kill a Mockingbird", Author = "Harper Lee", Genre = Genres.Fiction },
                new Book { Id = 3, BookName = "1984", Author = "George Orwell", Genre = Genres.ScienceFiction },
                new Book { Id = 4, BookName = "Pride and Prejudice", Author = "Jane Austen", Genre = Genres.Romance },
                new Book { Id = 5, BookName = "The Catcher in the Rye", Author = "J.D. Salinger", Genre = Genres.Fiction }
            );

            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "1", Description = "Administrator role" },
                new ApplicationRole { Id = "2", Name = "User", NormalizedName = "USER", ConcurrencyStamp = "2", Description = "Standard user role" }
            );

            modelBuilder.Entity<Book>().Property(b => b.BookName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Book>().Property(b => b.Author).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Book>().Property(b => b.Genre).IsRequired();
        }
    }
}
