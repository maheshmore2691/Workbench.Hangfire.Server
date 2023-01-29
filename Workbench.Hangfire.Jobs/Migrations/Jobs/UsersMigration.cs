using Workbench.Hangfire.Jobs.Migrations.Interfaces;

namespace Workbench.Hangfire.Jobs.Migrations.Jobs
{
    public class UsersMigration : IUsersMigration
    {
        public Task ExecuteAsync()
        {
            Console.WriteLine("Successfully completed!");

            return Task.CompletedTask;
        }
    }
}
