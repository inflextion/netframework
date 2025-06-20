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
        ProductEntity Create(ProductEntity product);
        ProductEntity GetById(int id);
        List<ProductEntity> GetActive();
        List<ProductEntity> SearchByName(string name);
        ProductEntity Update(ProductEntity product);
        void Delete(int id);
    }
}
