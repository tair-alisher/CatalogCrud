using CatalogCrud.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace CatalogCrud.DAL.EF
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<CatalogContext>
    {
        protected override void Seed(CatalogContext db)
        {
            var catalogs = new List<Catalog>
            {
                new Catalog
                {
                    Id = Guid.NewGuid(),
                    Name = "oked3"
                },
                new Catalog
                {
                    Id = Guid.NewGuid(),
                    Name = "SOATO"
                },
                new Catalog
                {
                    Id = Guid.NewGuid(),
                    Name = "SPROPF"
                },
                new Catalog
                {
                    Id = Guid.NewGuid(),
                    Name = "SPRSEK"
                },
                new Catalog
                {
                    Id = Guid.NewGuid(),
                    Name = "SPRSOB"
                }
            };

            var fields = new List<Field>
            {
                new Field
                {
                    Id = Guid.NewGuid(),
                    Name = "KOD"
                },
                new Field
                {
                    Id = Guid.NewGuid(),
                    Name = "KODA"
                },
                new Field
                {
                    Id = Guid.NewGuid(),
                    Name = "NAME"
                },
                new Field
                {
                    Id = Guid.NewGuid(),
                    Name = "Nace"
                }
            };

            foreach (var catalog in catalogs)
                db.Catalogs.Add(catalog);

            foreach (var field in fields)
                db.Fields.Add(field);

            db.SaveChanges();
        }
    }
}
