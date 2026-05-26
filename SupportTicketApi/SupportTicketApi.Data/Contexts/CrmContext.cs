using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Core.Models;

namespace SupportTicketApi.Data.Contexts;

public class CrmContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyType> CompanyTypes => Set<CompanyType>();
    public DbSet<ActivityType> ActivityTypes => Set<ActivityType>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<Discipline> Disciplines => Set<Discipline>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<SwecoUser> SwecoUsers => Set<SwecoUser>();
    public DbSet<SwecoUserRole> SwecoUserRoles => Set<SwecoUserRole>();
    public DbSet<LeadPhase> LeadPhases => Set<LeadPhase>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.CompanyTypeId)
                .IsRequired();

            entity.Property(e => e.Name)
                .IsRequired();

            entity.HasOne(e => e.CompanyType)
                .WithMany(e => e.Companies)
                .HasForeignKey(e => e.CompanyTypeId);
        });

        modelBuilder.Entity<CompanyType>(entity =>
        {
            entity.Property(e => e.Value)
                .IsRequired();

            entity.HasIndex(e => e.Value)
                .IsUnique();

            entity.HasMany(e => e.Companies)
                .WithOne(e => e.CompanyType)
                .HasForeignKey(e => e.CompanyTypeId);
        });

        modelBuilder.Entity<ActivityType>(entity =>
        {
            entity.Property(e => e.Value)
                .IsRequired();
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.CompanyId)
                .IsRequired();

            entity.Property(e => e.ActivityTypeId)
                .IsRequired();

            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId);

            entity.HasOne(e => e.ActivityType)
                .WithMany()
                .HasForeignKey(e => e.ActivityTypeId);

            entity.HasOne(e => e.Contact)
                .WithMany()
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Lead)
                .WithMany()
                .HasForeignKey(e => e.LeadId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<LeadPhase>(entity =>
        {
            entity.Property(e => e.Value)
                .IsRequired();
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.Property(e => e.Value)
                .IsRequired();

            entity.HasMany(e => e.Leads)
                .WithMany(e => e.Disciplines);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.Property(e => e.FirstName)
                .IsRequired();

            entity.Property(e => e.LastName)
                .IsRequired();

            entity.Property(e => e.CompanyId)
                .IsRequired();

            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId);

            entity.HasOne(e => e.DirectManager)
                .WithMany()
                .HasForeignKey(e => e.DirectManagerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Lead>(entity =>
        {
            entity.Property(e => e.Subject)
                .IsRequired();

            entity.Property(e => e.CompanyId)
                .IsRequired();

            entity.HasOne(e => e.Contact)
                .WithMany()
                .HasForeignKey(e => e.ContactId);

            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId);

            entity.HasMany(e => e.Disciplines)
                .WithMany(e => e.Leads);

            entity.HasOne(e => e.LeadPhase)
                .WithMany(e => e.Leads)
                .HasForeignKey(e => e.LeadPhaseId);
        });

        modelBuilder.Entity<SwecoUser>(entity =>
        {
            entity.Property(e => e.SwecoId)
                .IsRequired()
                .HasMaxLength(12);

            entity.HasIndex(e => e.SwecoId)
                .IsUnique();

            entity.HasMany(e => e.SwecoUserRoles)
                .WithMany(e => e.SwecoUsers);
        });

        modelBuilder.Entity<SwecoUserRole>(entity =>
        {
            entity.HasMany(e => e.SwecoUsers)
                .WithMany(e => e.SwecoUserRoles);
        });
    }
}