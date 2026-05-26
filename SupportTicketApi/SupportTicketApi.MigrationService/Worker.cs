using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using SupportTicketApi.Data.Contexts;
using SupportTicketApi.Core.Models;

namespace SupportTicketApi.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(
        CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity(
            "Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CrmContext>();

            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(
        CrmContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(
        CrmContext dbContext, CancellationToken cancellationToken)
    {
        List<ActivityType> activityTypes = [
            new()
            {
                Value = "Leadopvolging"
            },
            new()
            {
                Value = "Contactmoment"
            },
            new()
            {
                Value = "Samenwerking"
            },
            new()
            {
                Value = "Bezoek"
            },
        ];

        List<CompanyType> companyTypes = [
            new()
            {
                Value = "Aannemer"
            },
            new()
            {
                Value = "Beheerder"
            },
            new()
            {
                Value = "Gemeente"
            },
            new()
            {
                Value = "Provincie"
            },
            new()
            {
                Value = "IB"
            },
        ];

        List<LeadPhase> leadPhases = [
            new()
            {
                Value = "Well-done"
            },
            new()
            {
                Value = "Medium"
            },
            new()
            {
                Value = "Rare"
            },
        ];

        List<Discipline> disciplines = [
            new()
            { 
                Value = "TB"
            },
            new()
            {
                Value = "RVT"
            },
            new()
            {
                Value = "TEV"
            },
            new()
            {
                Value = "Railstudies"
            },
            new()
            {
                Value = "Realisatiemanagement"
            },
            new()
            {
                Value = "Baan & Spoor"
            },
            new()
            {
                Value = "Bovenleiding"
            },
            new()
            {
                Value = "Digitalisering"
            },
        ];

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database
                .BeginTransactionAsync(cancellationToken);

            await dbContext.ActivityTypes.AddRangeAsync(activityTypes, cancellationToken);
            await dbContext.CompanyTypes.AddRangeAsync(companyTypes, cancellationToken);
            await dbContext.Disciplines.AddRangeAsync(disciplines, cancellationToken);
            await dbContext.LeadPhases.AddRangeAsync(leadPhases, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}