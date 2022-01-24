using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
       
        }

        public DbSet<Product> Products {get; set;}

        public DbSet<ProductBrand> ProductBrands {get; set;}

        public DbSet<ProductType> ProductTypes {get; set;}


        protected override void OnModelCreating(ModelBuilder modelbuilder){
          base.OnModelCreating(modelbuilder);
          modelbuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          // my database is sqlserver 
          // sqlite database does not support decimal 
          // so we need to convert it manually to double 
          // this section is just educational

          if( Database.ProviderName == "Microsoft.EntityFrameworkCore.SqLite"){
               
               foreach( var entityType in modelbuilder.Model.GetEntityTypes()){
                   
                   var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
                   foreach(var property in properties){
                     modelbuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                   }
               }
          }

        }
    }
}