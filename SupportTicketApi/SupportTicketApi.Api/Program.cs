using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using SupportTicketApi.Api.Mappers;
using SupportTicketApi.Core.Models.Enums;
using SupportTicketApi.Data.Authorization;
using SupportTicketApi.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDataServices(connectionName: "mydatabase");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TODO: use environment here!

        // TODO: perhaps use this url? 
        // https://login.swecogroup.com/adfs/oauth2/authorize/wia?client_id=31d5900d-1424-4f3b-aa8c-c6314a8eccea&redirect_uri=https%3A%2F%2Fidentity.gw1.prod.kubernetes.sweco.se%2Fsignin-oidc&response_type=code&scope=openid%20profile&code_challenge=pTgb6XNqVXNfDhdUPX_Bg8iwAcwwj95Ex1QB4K749-k&code_challenge_method=S256&response_mode=form_post&nonce=639039710356026179.NjZlYmY4ZGMtYTQzNi00Yjg0LTkyYzEtNzU0MmI4Mjg0YTVkNzVhYzRkODItYzc4Yi00OGNiLTllMGEtY2Q3NjcyNjkxYzVi&state=CfDJ8AvBXXnIMO5Ft3VSKULo4Z2Kbz0ZkKPhlhK2v70HAuiAOgVQsJIQnW6CPTVLsUa684Vep5tyAaEYcblipY9QZ1BrfIbu2B9TJZ1bVZCfsewLJSqbcEK4F0ARYYEPs-XP0GvZiD-En5KDRNuwKOezWhbQkAH2sMrFp7_579rPNoZOEjyC4p1E21daV7h4J90dQ6SPnmQmejgFjcfjRdfH_bbCtcJ08z4L3lxPzm3yltHRdM7WKuXniHDppEX4_clKmBVmGZ7BAfY8kxkHBJ7u4umgyPkztiXfnvkiAQAgR_XSZ7zuCGJakg0x89p48rXhl3dgoRRzEpsEYMXCnhq99wz-0YmdMpXl4f8WRborb-jA97hWfcwniw-hiij-fSocOzFf4PMhL5F25Ym_n4cUS-OtO6VoeUaGre2AHzD5Yy5EHnPZqtpJYqpRqQfoMMeDTwxUl11HUyjh9VS-kP0t0QGcdXpJpDiQ71f6KnOIrgltAV8B6SYDXpEDrfgdIa5ePBlBedcxJ1uNfoxBkCHzijpHUsRopiv_nYWHGIoY2gYeMWR2AcH5Dpt869MEL6EvU0t6QXGKl5rUcbaCbjvOFXSCrO0cS7F0huCl2oWDHA7jO-01XyY_E8OrmR3yNDei-xGtF5MopZO3MVDskpVx9hJks1fLuA5nVdVjflu5OEXfWR31_cXeDB0qCbPF_KsnMiyHtbCxRKkVdp_Q_DBEpM1cOrAdA2BMCwpeD7mEhBcJIQjkmI6V8orXaNPZUHfJsWAsw4-diKQrEa6GlDBLl_B9Bj99970TL2eiDwhZ0UCWWBskvEN2VLnsmua-CRVHmVeyLXIpDYyKZzuff3pJ8l_eSdWYiZM2g6K61_kZMFwU_wJAQzSmpVF2sugRhPjZzchmvSjWZ6nExFh1nVJShiQ&x-client-SKU=ID_NETSTANDARD2_0&x-client-ver=5.6.0.0&client-request-id=5bca3129-926d-4007-aa41-0080010000ca

        // e.g., https://login.microsoftonline.com/{tenant-id}/v2.0
        options.Authority = "https://identity.gw1.prod.kubernetes.sweco.se";

        options.Audience = "nl_connect_rail";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://identity.gw1.prod.kubernetes.sweco.se",
            ValidateAudience = true,
            ValidAudiences =
            [
                "employeeservice",
                "organizationservice",
                "projectservice",
                "https://identity.gw1.prod.kubernetes.sweco.se/resources"
            ],
            ValidateLifetime = true
        };
    });

builder.Services.AddHttpContextAccessor(); // Add this line
builder.Services.AddScoped<IAuthorizationHandler, RoleHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsUser", policy =>
        policy.Requirements.Add(new RoleRequirement(Role.User)))
    .AddPolicy("IsAdmin", policy =>
        policy.Requirements.Add(new RoleRequirement(Role.Admin)));

builder.AddServiceDefaults();

builder.Services.AddControllers(options =>
{
    var rolePolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddRequirements(new RoleRequirement(Role.User))
        .Build();

    options.Filters.Add(new AuthorizeFilter(rolePolicy));
}).AddNewtonsoftJson(); // NOTE: we use NewtonsoftJson for PATCH support

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((options) =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Rail and Stations CRM API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your Microsoft Entra ID token. Example: '12345abcdef'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

builder.Services.AddSingleton<Profile, MappingProfile>();

builder.Services.AddSingleton<AutoMapper.IConfigurationProvider>(sp =>
    new MapperConfiguration(cfg =>
    {
        cfg.AllowNullCollections = true;
        cfg.AddProfile<MappingProfile>();
    }, new NullLoggerFactory()));


builder.Services.AddScoped<IMapper>(sp =>
    new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // NOTE: Since we are using migrations, this line is no longer needed
    //app.EnsureDatabaseCreated();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();