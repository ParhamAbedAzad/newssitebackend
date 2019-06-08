using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NewsSiteBackEnd.Models
{
    public partial class NEWS_SITEContext : DbContext
    {
        public NEWS_SITEContext()
        {
        }

        public NEWS_SITEContext(DbContextOptions<NEWS_SITEContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsPhoto> NewsPhoto { get; set; }
        public virtual DbSet<RadioUrl> RadioUrl { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=185.252.30.32;Database=NEWS_SITE;Persist Security Info=True;User ID=izad;Password=Izadizadi1742");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(64);

                entity.Property(e => e.PhotoUrl)
                    .HasColumnName("photoURL")
                    .HasMaxLength(200);

                entity.Property(e => e.Privilege)
                    .HasColumnName("privilege")
                    .HasMaxLength(100);

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasMaxLength(128);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Body)
                    .HasColumnName("body")
                    .HasMaxLength(500);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NewsId).HasColumnName("newsId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.NewsId)
                    .HasConstraintName("FK_Comments_News");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Comments_users");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminId).HasColumnName("adminId");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("dateAdded")
                    .HasColumnType("datetime");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("ntext");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK_News_Admins");
            });

            modelBuilder.Entity<NewsPhoto>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NewsId).HasColumnName("newsID");

                entity.Property(e => e.PhotoUrl)
                    .HasColumnName("photoURL")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.HasOne(d => d.News)
                    .WithMany(p => p.NewsPhoto)
                    .HasForeignKey(d => d.NewsId)
                    .HasConstraintName("FK_NewsPhoto_News");
            });

            modelBuilder.Entity<RadioUrl>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NewsId).HasColumnName("newsID");

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasMaxLength(30);

                entity.HasOne(d => d.News)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.NewsId)
                    .HasConstraintName("FK_Tags_News");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(800);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasColumnName("firstName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(64);

                entity.Property(e => e.PhotoUrl)
                    .HasColumnName("photoURL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasMaxLength(128);

                entity.Property(e => e.TelNumber)
                    .HasColumnName("telNumber")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
