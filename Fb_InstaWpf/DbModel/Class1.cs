using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fb_InstaWpf.DbModel
{
    public class SQLiteConfiguration : DbConfiguration
    {
        public SQLiteConfiguration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }


    [Table("Users")]
    public class Users
    {
        [Column("Id", TypeName = "INTEGER")]
        [Key]
        public int Id { get; set; }

        [Column("UserName", TypeName = "VARCHAR")]
        public string UserName { get; set; }

        [Column("Password", TypeName = "VARCHAR")]
        public double Password { get; set; }
        [Column("FacebookId", TypeName = "VARCHAR")]
        public double FacebookId { get; set; }
    }

    public class DatabaseContext : DbContext
    {
        public DatabaseContext() :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = @"C:\Users\Windows10\source\repos\WpfInsta2\FbInstaRepository\Fb_InstaWpf\DBFbInstaComment\Facebookdb.db", ForeignKeys = true }.ConnectionString
            }, true)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Users> Users { get; set; }
    }
}
