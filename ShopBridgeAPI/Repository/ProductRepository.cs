using ShopBridgeAPI.Data;
using ShopBridgeAPI.Models;
using ShopBridgeAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateProduct(Product product)
        {
            _db.Products.Add(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _db.Products.Remove(product);
            return Save();
        }

        public Product GetProduct(int id)
        {
            return _db.Products.FirstOrDefault(a => a.Id == id);
        }

        public ICollection<Product> GetProducts()
        {
            return _db.Products.OrderBy(a => a.Name).ToList();
        }

        public bool ProductExists(int id)
        {
            return _db.Products.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateProduct(Product product)
        {
            _db.Products.Update(product);
            return Save();
        }
    }
}
