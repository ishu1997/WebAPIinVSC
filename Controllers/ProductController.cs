using Microsoft.AspNetCore.Mvc;
using WebAPIinVSC.Models;

namespace WebAPIinVSC.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    public readonly LearnDbContext learnDbContext;


    public ProductController(LearnDbContext _learnDbContext)
    {
        this.learnDbContext = _learnDbContext;
    }

    [HttpGet("GetAllProducts")]
    public IActionResult Get()
    {
        var products = this.learnDbContext.Products.ToList();
        return Ok(products);
    }

    [HttpGet("GetById/{Id}")]
    public IActionResult Get(int Id)
    {
        var products = this.learnDbContext.Products.FirstOrDefault(product => product.Id == Id);
        if (products != null)
        {
            return Ok(products);
        }
        return Ok(false);
    }

    [HttpDelete("Remove/{Id}")]
    public IActionResult Remove(int Id)
    {
        var product = this.learnDbContext.Products.FirstOrDefault(product => product.Id == Id);
        if (product != null)
        {
            this.learnDbContext.Products.Remove(product);
            this.learnDbContext.SaveChanges();
            return Ok(true);
        }
        return Ok(false);
    }

    [HttpPost("Create")]
    public IActionResult Create([FromBody] Product product)
    {
        var ExistingProduct = this.learnDbContext.Products.FirstOrDefault(exist => exist.Name == product.Name);
        if (ExistingProduct == null)
        {
            this.learnDbContext.Products.Add(product);
            this.learnDbContext.SaveChanges();
            return Ok(true);
        }
        return Ok(false);
    }

}
