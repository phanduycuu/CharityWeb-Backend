using Charity.Models;
using Microsoft.EntityFrameworkCore;

namespace Charity.Data;

public partial class CharityContext : DbContext
{
    public CharityContext()
    {
    }

    public CharityContext(DbContextOptions<CharityContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<CampaignUpdate> CampaignUpdates { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Subscription>().HasIndex(s => s.Email).IsUnique();
        modelBuilder.Entity<User>()
           .HasIndex(u => u.Email)
           .IsUnique();

        modelBuilder.Entity<Subscription>()
            .HasIndex(s => s.Email)
            .IsUnique();

        modelBuilder.Entity<Campaign>()
            .HasOne(c => c.CreatedBy)
            .WithMany(u => u.CreatedCampaigns)
            .HasForeignKey(c => c.CreatedById);

        modelBuilder.Entity<Donation>()
            .HasOne(d => d.Campaign)
            .WithMany(c => c.Donations)
            .HasForeignKey(d => d.CampaignId);

        modelBuilder.Entity<Donation>()
            .HasOne(d => d.Donor)
            .WithMany(u => u.Donations)
            .HasForeignKey(d => d.DonorId)
            .IsRequired(false);

        modelBuilder.Entity<CampaignUpdate>()
            .HasOne(u => u.Campaign)
            .WithMany(c => c.Updates)
            .HasForeignKey(u => u.CampaignId);
        modelBuilder.Entity<Campaign>()
        .HasOne(c => c.Category)
        .WithMany(cat => cat.Campaigns)
        .HasForeignKey(c => c.CategoryId);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
