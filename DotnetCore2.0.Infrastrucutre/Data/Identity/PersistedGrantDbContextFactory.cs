using System;
using System.IO;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DotnetCore2.Infrastrucutre.Data.Identity
{
    public class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
         
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
             string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(new DirectoryInfo(Environment.CurrentDirectory).FullName)//System.AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString("Default");
            optionsBuilder.UseSqlServer(connstr, 
              sql => sql.MigrationsAssembly(typeof(PersistedGrantDbContextFactory).GetTypeInfo().Assembly.GetName().Name));
            return new PersistedGrantDbContext(optionsBuilder.Options, new OperationalStoreOptions());

            
        }

        


    }
}
