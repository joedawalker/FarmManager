using EquipmentApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace EquipmentApi
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main( string[] args )
        {
            var host = CreateHostBuilder( args ).Build();

            using ( var scope = host.Services.CreateScope() )
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<EquipmentApiContext>();

                    context.Database.Migrate();
                }
                catch ( Exception ex )
                {
                    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError( ex, "An error occurred while seeding the DB." );
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder( string[] args ) =>
            Host.CreateDefaultBuilder( args )
                .ConfigureWebHostDefaults( webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 } );
    }
#pragma warning restore CS1591
}
