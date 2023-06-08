using Admin_Console.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Admin_Console.Data
{
    public class BackEndApplicationDbContext : IdentityDbContext<BackEndApplicationUser, IdentityRole, string>
    {
        public BackEndApplicationDbContext(DbContextOptions<BackEndApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("TcuServerConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}