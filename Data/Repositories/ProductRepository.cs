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
    /// Repository for managing <see cref="Product"/> entities in the database.
    /// Provides methods to create and retrieve products.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        public Product Create(Product product)
        {
            using var context = DbContextFactory.CreateContext();
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public Product GetById(int id)
        {
            using var context = DbContextFactory.CreateContext();
            return context.Products.Find(id);
        }

        public List<Product> GetActive()
        {
            using var context = DbContextFactory.CreateContext();
            return context.Products.Where(p => p.IsActive).ToList();
        }

        public List<Product> SearchByName(string name)
        {
            using var context = DbContextFactory.CreateContext();
            return context.Products
                .Where(p => p.Name.Contains(name) && p.IsActive)
                .ToList();
        }

        public Product Update(Product product)
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
