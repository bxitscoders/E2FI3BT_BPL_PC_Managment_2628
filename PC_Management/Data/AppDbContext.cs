using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using PCVerwaltung.Classes;

namespace PCVerwaltung.Data
{
    /// <summary>
    /// Entity Framework Core DbContext für die SQLite-Datenbank pcverwaltung.db.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Case> Cases { get; set; } = null!;
        public DbSet<CPU> CPUs { get; set; } = null!;
        public DbSet<Mainboard> Mainboards { get; set; } = null!;
        public DbSet<Ram> Rams { get; set; } = null!;
        public DbSet<SSD> SSDs { get; set; } = null!;
        public DbSet<PC> PCs { get; set; } = null!;

        public static string DbPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pcverwaltung.db");

        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={DbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Case>().ToTable("Cases");
            modelBuilder.Entity<CPU>().ToTable("CPUs");
            modelBuilder.Entity<Mainboard>().ToTable("Mainboards");
            modelBuilder.Entity<Ram>().ToTable("Rams");
            modelBuilder.Entity<SSD>().ToTable("SSDs");

            modelBuilder.Entity<PC>(entity =>
            {
                entity.ToTable("PCs");
                entity.HasKey(p => p.Id);

                // Optionale Fremdschlüssel zu Komponenten
                entity.HasOne(p => p.Case)
                      .WithMany()
                      .HasForeignKey("CaseId")
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(p => p.Cpu)
                      .WithMany()
                      .HasForeignKey("CpuId")
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(p => p.Mainboard)
                      .WithMany()
                      .HasForeignKey("MainboardId")
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(p => p.Ram)
                      .WithMany()
                      .HasForeignKey("RamId")
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(p => p.Ssd)
                      .WithMany()
                      .HasForeignKey("SsdId")
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
