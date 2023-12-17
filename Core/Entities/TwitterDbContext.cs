using Core.Configs;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

public partial class TwitterDbContext :DbContext
{
    public TwitterDbContext()
    {
    }

    public TwitterDbContext(DbContextOptions<TwitterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Follower> Followers { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<UserTweets> UserTweets { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(ConfigsManager.TryGet(AppSettingsConfig.ConnectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Follower>(entity =>
        {
            entity.ToTable("Follower");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.HasOne(d => d.FollowedByUser)
                .WithMany()
                .HasForeignKey(d => d.FollowedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Follower_User1");

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Follower_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.Email).HasMaxLength(250);

            entity.Property(e => e.Guid).HasMaxLength(50);

            entity.Property(e => e.Password).HasMaxLength(250);

            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserTweets>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.TweetContent).HasMaxLength(250);

            entity.Property(e => e.TweetGuid).HasMaxLength(50);

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTweets_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
