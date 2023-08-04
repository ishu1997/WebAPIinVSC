

using Microsoft.AspNetCore.WebUtilities;
using WebAPIinVSC.Models;

public class ProductRepo : IproductRepo
{
    public readonly LearnDbContext learnDbContext;
    public ProductRepo(LearnDbContext dbContext)
    {
        this.learnDbContext = dbContext;
    }
    public bool CreateProduct(Product product)
    {
        var ExistingProduct = this.learnDbContext.Products.FirstOrDefault(exist => exist.Name == product.Name);
        if (ExistingProduct == null)
        {
            this.learnDbContext.Products.Add(product);
            this.learnDbContext.SaveChanges();
            return true;
        }
        return false;
    }

    public List<Product> GetAll()
    {
        var products = this.learnDbContext.Products.ToList();
        return products;
    }

    public Product? GetById(int id)
    {
        var product = this.learnDbContext.Products.FirstOrDefault(product => product.Id == id);
        if (product != null)
        {
            return product;
        }
        return null;
    }

    public bool RemoveById(int id)
    {
        var product = this.learnDbContext.Products.FirstOrDefault(product => product.Id == id);
        if (product != null)
        {
            this.learnDbContext.Products.Remove(product);
            this.learnDbContext.SaveChanges();
            return true;
        }
        return false;
    }
}