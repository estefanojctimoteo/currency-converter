using System.IO;
using Currency_Converter.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Currency_Converter.Infra.Data.Mappings;
using Currency_Converter.Domain.Conversions;
using Currency_Converter.Domain.Users;
using Currency_Converter.Infra.Data.Helper;
using Microsoft.Extensions.Logging;

namespace Currency_Converter.Infra.Data.Context
{
    public class ConversionContext : DbContext
    {
        static ConversionContext() {}        

        public DbSet<Conversion> Conversion { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new ConversionMapping());
            modelBuilder.AddConfiguration(new UserMapping());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            var lf = new LoggerFactory();
            lf.AddProvider(new MyLoggerProvider());
            optionsBuilder.UseLoggerFactory(lf);
#endif

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlite(config.GetConnectionString("SQLite"));
        }
    }
}