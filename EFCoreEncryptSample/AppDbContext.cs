using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFCoreEncryptSample
{
    public class AppDbContext : DbContext
    {
        private readonly IEncryptionProvider _provider;

        // Get key and IV from a Base64String or any other ways.
        // You can generate a key and IV using "AesProvider.GenerateKey()"
        //Can use a 128bits, 192bits or 256bits key

        //private readonly byte[] _encryptionKey = new byte[]
        //{
        //    0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF,
        //    0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10
        //};
        //private readonly byte[] _encryptionIV = new byte[]
        //             {
        //                 0xF1, 0xD2, 0x03, 0x04, 0x05, 0xA7, 0xB7, 0xC8
        //             };

        private readonly byte[] _encryptionKey = Encoding.UTF8.GetBytes("abc12#_5D&141521");
        private readonly byte[] _encryptionIV = Encoding.UTF8.GetBytes("D*vb_3$741SEa@km");
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            _provider = new AesProvider(this._encryptionKey, this._encryptionIV);

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
            //fluent
            EntityTypeBuilder<Student> studentEntityBuilder = modelBuilder.Entity<Student>();

            studentEntityBuilder.Property(x => x.Email)
                .IsEncrypted(StorageFormat.Binary);

            studentEntityBuilder.Property(x => x.Phone)
                .IsEncrypted(StorageFormat.Base64);

            if (_provider is not null)
            {
                modelBuilder.UseEncryption(_provider);
            }

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Student> Students { get; set; }
    }
}
