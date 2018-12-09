using CatalogCrud.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CatalogCrud.DAL.EF
{
    public class CatalogContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Value> Values { get; set; }

        static CatalogContext() { Database.SetInitializer<CatalogContext>(null); }
        public CatalogContext(string connection) : base(connection) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Catalog>()
            //    .HasMany<Field>(c => c.Fields)
            //    .WithMany(f => f.Catalogs)
            //    .Map(cf =>
            //    {
            //        cf.MapLeftKey("CatalogId");
            //        cf.MapRightKey("FieldId");
            //        cf.ToTable("CatalogField");
            //    });

            //modelBuilder.Entity<Value>()
            //    .HasRequired(v => v.Field)
            //    .WithMany(f => f.Values)
            //    .HasForeignKey(v => v.FieldId);

            //modelBuilder.Entity<Value>()
            //    .HasRequired(v => v.Catalog)
            //    .WithMany(c => c.Values)
            //    .HasForeignKey(v => v.CatalogId);
        }
    }
}
