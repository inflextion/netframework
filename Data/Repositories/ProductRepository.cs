using atf.Data.Models;
using atf.Data.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atf.Data.Repositories
{
    /// <summary>
    /// Repository for managing <see cref="ProductEntity"/> entities in the database.
    /// Provides methods for full CRUD operations including create, retrieve, update, delete, and search functionality.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        public ProductEntity Create(ProductEntity product)
        {
            using var context = DbContextFactory.CreateContext();
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public ProductEntity GetById(int id)
        {
            using var context = DbContextFactory.CreateContext();
            return context.Products.Find(id);
        }

        public List<ProductEntity> GetActive()
        {
            using var context = DbContextFactory.CreateContext();
            return context.Products.Where(p => p.IsActive).ToList();
        }

        public List<ProductEntity> SearchByName(string name)
        {
            using var context = DbContextFactory.CreateContext();
            return context.Products
                .Where(p => p.Name.Contains(name) && p.IsActive)
                .ToList();
        }

        public ProductEntity Update(ProductEntity product)
        {
            using var context = DbContextFactory.CreateContext();
            context.Products.Update(product);
            context.SaveChanges();
            return product;
        }

        public void Delete(int id)
        {
            using var context = DbContextFactory.CreateContext();
            var product = context.Products.Find(id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }
    }
}
