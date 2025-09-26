using Microsoft.EntityFrameworkCore;
using MyLibraryTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }
        public DbSet<Review> Reviews { get; set; }


        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=ALI\SQLEXPRESS;Database=MyLibraryTest;Integrated Security=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .Property<string>("Password");

            //modelBuilder.Entity<User>()
            //    .HasKey(u => u.Id);

            //modelBuilder.Entity<User>()
            //    .Property(u => u.Username)
            //    .HasMaxLength(50);



            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);

                entity.Property<string>("Password").IsRequired();

                entity.Property(u => u.Username)
                    .HasMaxLength(50).IsRequired();

                entity.HasMany(u => u.Reviews)          
                .WithOne(r => r.User)          
                .HasForeignKey(r => r.UserId);

                entity.HasMany(u => u.BorrowedBooks)   
                .WithOne(bb => bb.User)         
                .HasForeignKey(bb => bb.UserId);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title)
                .HasMaxLength(50).IsRequired();

                 entity.HasOne(b => b.Category)    
                .WithMany(c => c.Books)       
                .HasForeignKey(b => b.CategoryId);

                entity.HasMany(b => b.Reviews)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId);
                

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                .HasMaxLength(50).IsRequired();

            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Comment).HasMaxLength(300).IsRequired();
                entity.Property(r => r.Rating).IsRequired();
            });

            modelBuilder.Entity<BorrowedBook>(entity =>
            {
                entity.ToTable("BorrowedBooks");
                entity.HasKey(bb => bb.Id);
                entity.Property(bb => bb.BorrowDate).IsRequired();

            });


            
        }
    }
}
