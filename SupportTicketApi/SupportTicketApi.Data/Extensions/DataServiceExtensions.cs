using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SupportTicketApi.Data.Contexts;
using SupportTicketApi.Data.Repositories;

namespace SupportTicketApi.Data.Extensions;

public static class DataServiceExtensions
{
    public static IHostApplicationBuilder AddDataServices(
            this IHostApplicationBuilder builder,
            string connectionName)
    {
        builder.AddNpgsqlDbContext<CrmContext>(connectionName: connectionName);

        builder.Services.AddScoped<CompanyRepository>();
        builder.Services.AddScoped<CompanyTypeRepository>();
        builder.Services.AddScoped<ActivityRepository>();
        builder.Services.AddScoped<ActivityTypeRepository>();
        builder.Services.AddScoped<ContactRepository>();
        builder.Services.AddScoped<LeadRepository>();
        builder.Services.AddScoped<LeadPhaseRepository>();
        builder.Services.AddScoped<DisciplineRepository>();
        builder.Services.AddScoped<SwecoUserRepository>();
        builder.Services.AddScoped<SwecoUserRoleRepository>();

        return builder;
    }
}
