var builder = DistributedApplication.CreateBuilder(args);

var postgresdb = builder.AddConnectionString("mydatabase");

var migrations = builder.AddProject<Projects.SupportTicketApi_MigrationService>("migrations")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.AddProject<Projects.SupportTicketApi_Api>("api")
    .WithReference(postgresdb)
    .WithReference(migrations)
    .WaitForCompletion(migrations);

builder.Build().Run();
