using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

public partial class DbprojectContext : DbContext
{
    public DbprojectContext()
    {
    }

    public DbprojectContext(DbContextOptions<DbprojectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<Journal> Journals { get; set; }

    public virtual DbSet<Publish> Publishes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DBProject");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "pg_cron");

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("article_pkey");

            entity.Property(e => e.AddressedPopulation).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Challenge).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Doi).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Focus).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.License).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.StudyType).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Url).HasDefaultValueSql("NULL::character varying");

            entity.HasMany(d => d.Citeds).WithMany(p => p.Citings)
                .UsingEntity<Dictionary<string, object>>(
                    "Cite",
                    r => r.HasOne<Article>().WithMany()
                        .HasForeignKey("CitedId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("cite_cited_id_fkey"),
                    l => l.HasOne<Article>().WithMany()
                        .HasForeignKey("CitingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("cite_citing_id_fkey"),
                    j =>
                    {
                        j.HasKey("CitingId", "CitedId").HasName("cite_pkey");
                        j.ToTable("cite");
                        j.HasIndex(new[] { "CitingId", "CitedId" }, "cite_citing_id_cited_id_idx");
                        j.IndexerProperty<string>("CitingId")
                            .HasMaxLength(100)
                            .HasColumnName("citing_id");
                        j.IndexerProperty<string>("CitedId")
                            .HasMaxLength(100)
                            .HasColumnName("cited_id");
                    });

            entity.HasMany(d => d.Citings).WithMany(p => p.Citeds)
                .UsingEntity<Dictionary<string, object>>(
                    "Cite",
                    r => r.HasOne<Article>().WithMany()
                        .HasForeignKey("CitingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("cite_citing_id_fkey"),
                    l => l.HasOne<Article>().WithMany()
                        .HasForeignKey("CitedId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("cite_cited_id_fkey"),
                    j =>
                    {
                        j.HasKey("CitingId", "CitedId").HasName("cite_pkey");
                        j.ToTable("cite");
                        j.HasIndex(new[] { "CitingId", "CitedId" }, "cite_citing_id_cited_id_idx");
                        j.IndexerProperty<string>("CitingId")
                            .HasMaxLength(100)
                            .HasColumnName("citing_id");
                        j.IndexerProperty<string>("CitedId")
                            .HasMaxLength(100)
                            .HasColumnName("cited_id");
                    });
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("author_pkey");

            entity.Property(e => e.Country).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Email).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Institution).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Lab).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.PostCode).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Settlement).HasDefaultValueSql("NULL::character varying");

            entity.HasMany(d => d.Articles).WithMany(p => p.AuthorNames)
                .UsingEntity<Dictionary<string, object>>(
                    "Write",
                    r => r.HasOne<Article>().WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("write_article_id_fkey"),
                    l => l.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorName")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("write_author_name_fkey"),
                    j =>
                    {
                        j.HasKey("AuthorName", "ArticleId").HasName("write_pkey");
                        j.ToTable("write");
                        j.HasIndex(new[] { "ArticleId" }, "idx_write_aeid");
                        j.HasIndex(new[] { "AuthorName" }, "idx_write_auname");
                        j.IndexerProperty<string>("AuthorName")
                            .HasMaxLength(200)
                            .HasColumnName("author_name");
                        j.IndexerProperty<string>("ArticleId")
                            .HasMaxLength(50)
                            .HasColumnName("article_id");
                    });
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ArticleId, e.Time }).HasName("history_pkey");

            entity.HasOne(d => d.Article).WithMany(p => p.Histories)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("history_article_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Histories)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("history_user_id_fkey");
        });

        modelBuilder.Entity<Journal>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("journal_pkey");

            entity.Property(e => e.Description).HasDefaultValueSql("NULL::character varying");
        });

        modelBuilder.Entity<Publish>(entity =>
        {
            entity.HasKey(e => new { e.JournalName, e.ArticleId }).HasName("publish_pkey");

            entity.Property(e => e.Pages).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Volume).HasDefaultValueSql("NULL::character varying");

            entity.HasOne(d => d.Article).WithMany(p => p.Publishes)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("publish_article_id_fkey");

            entity.HasOne(d => d.JournalNameNavigation).WithMany(p => p.Publishes)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("publish_journal_name_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Avatar).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Collage).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.Motto).HasDefaultValueSql("NULL::character varying");

            entity.HasMany(d => d.Articles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Collect",
                    r => r.HasOne<Article>().WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("collect_article_id_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("collect_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "ArticleId").HasName("collect_pkey");
                        j.ToTable("collect");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<string>("ArticleId")
                            .HasMaxLength(50)
                            .HasColumnName("article_id");
                    });

            entity.HasMany(d => d.JournalNames).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Subscribe",
                    r => r.HasOne<Journal>().WithMany()
                        .HasForeignKey("JournalName")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("subscribe_journal_name_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("subscribe_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "JournalName").HasName("subscribe_pkey");
                        j.ToTable("subscribe");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<string>("JournalName")
                            .HasMaxLength(200)
                            .HasColumnName("journal_name");
                    });
        });
        modelBuilder.HasSequence("jobid_seq", "cron");
        modelBuilder.HasSequence("runid_seq", "cron");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
