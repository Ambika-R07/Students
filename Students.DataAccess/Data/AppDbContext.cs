//using Microsoft.EntityFrameworkCore;
//using SMS.Domain.Entities;
//using System.Collections.Generic;
//using System.Reflection.Emit;

//namespace SMS.Infrastructure.Data
//{
//    public class AppDbContext : DbContext
//    {
//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//        public DbSet<Student> Students { get; set; } = null!;

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Student>(b =>
//            {
//                b.HasKey(s => s.Id);
//                b.Property(s => s.FirstName).HasMaxLength(100).IsRequired();
//                b.Property(s => s.LastName).HasMaxLength(100);
//                b.Property(s => s.Age).IsRequired();
//            });
//        }
//    }
//}
