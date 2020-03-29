using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Todo.Data;

namespace Todo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost webHostBuilder = CreateWebHostBuilder(args).Build();

            MigrateDb(webHostBuilder);

            webHostBuilder.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void MigrateDb(IWebHost webHostBuilder)
        {
            using (IServiceScope scope = webHostBuilder.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                using (var context = services.GetRequiredService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
