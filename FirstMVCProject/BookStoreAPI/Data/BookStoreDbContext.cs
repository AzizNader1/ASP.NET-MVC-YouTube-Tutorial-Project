using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.BookName).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Author).IsRequired().HasMaxLength(50);
                entity.Property(b => b.Genre).IsRequired();
                entity.Property(b => b.ImageUrl).HasMaxLength(500);

                entity.HasData(
                    new Book { Id = 1, BookName = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = Genres.Fiction, ImageUrl = "" },
                    new Book { Id = 2, BookName = "To Kill a Mockingbird", Author = "Harper Lee", Genre = Genres.Fiction, ImageUrl = "" },
                    new Book { Id = 3, BookName = "1984", Author = "George Orwell", Genre = Genres.ScienceFiction, ImageUrl = "" },
                    new Book { Id = 4, BookName = "Pride and Prejudice", Author = "Jane Austen", Genre = Genres.Romance, ImageUrl = "" },
                    new Book { Id = 5, BookName = "The Catcher in the Rye", Author = "J.D. Salinger", Genre = Genres.Fiction, ImageUrl = "" }
                );
            });
        }
    }
}
