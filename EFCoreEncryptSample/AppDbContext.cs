using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCoreEncryptSample
{
    public class AppDbContext : DbContext
    {
        private readonly IEncryptionProvider _provider;
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            this._provider = new GenerateEncryptionProvider("a555af9e93771a0721ceedb2a94010ea");

            var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (databaseCreator is not null)
            {
                if (!databaseCreator.CanConnect())
                {
                    databaseCreator.Create();
                }
                if (!databaseCreator.HasTables())
                {
                    databaseCreator.CreateTables();
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEncryption(_provider);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Student> Students { get; set; }
    }
}
