using ExpenseApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpenseApi
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var db = scope.ServiceProvider.GetRequiredService<ExpenseContext>())
                {
                    db.Database.Migrate();
                }
            }

            return host;
        }
    }
}
