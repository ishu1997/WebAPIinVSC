
using WebAPIinVSC.Models;

public interface IproductRepo
{

    public List<Product> GetAll();
    public Product? GetById(int id);
    public bool RemoveById(int id);
    public bool CreateProduct(Product product);

}