using Hangfire;
using Hangfire.SqlServer;
using Workbench.Hangfire.Jobs.Migrations.Interfaces;
using Workbench.Hangfire.Jobs.Migrations.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Configure Hangfire

builder.Services.AddHangfire(config => config
                                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                .UseSimpleAssemblyNameTypeSerializer()
                                .UseRecommendedSerializerSettings()
                                .UseSqlServerStorage(builder.Configuration.GetConnectionString("Hangfire"), new SqlServerStorageOptions
                                {
                                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                    QueuePollInterval = TimeSpan.Zero,
                                    UseRecommendedIsolationLevel = true,
                                    DisableGlobalLocks = true
                                })
                            );

builder.Services.AddHangfireServer();

// Configure job's dependencies
builder.Services.AddScoped<IUsersMigration, UsersMigration>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //endpoints.MapHangfireDashboard();
    endpoints.MapHangfireDashboard("/dashboard");
});

// Register job
RecurringJob.AddOrUpdate<IUsersMigration>(x => x.ExecuteAsync(), Cron.Never);

app.Run();
