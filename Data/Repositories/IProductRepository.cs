using atf.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atf.Data.Repositories
{
    public interface IProductRepository
    {
        Product Create(Product product);
        Product GetById(int id);
        List<Product> GetActive();
        List<Product> SearchByName(string name);
        Product Update(Product product);
        void Delete(int id);
    }
}
