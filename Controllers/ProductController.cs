using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIinVSC.Models;

namespace WebAPIinVSC.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    public IproductRepo _productRepo;


    public ProductController(IproductRepo productRepo)
    {
        _productRepo = productRepo;
    }

    [HttpGet("GetAllProducts")]
    public IActionResult Get()
    {
        var products = _productRepo.GetAll();
        return Ok(products);
    }

    [HttpGet("GetById/{Id}")]
    public IActionResult Get(int Id)
    {
        var products = _productRepo.GetById(Id);
        if (products != null)
        {
            return Ok(products);
        }
        return Ok(false);
    }

    [HttpDelete("Remove/{Id}")]
    public IActionResult Remove(int Id)
    {
        if (_productRepo.RemoveById(Id))
        {
            return Ok(true);
        }
        return Ok(false);
    }

    [HttpPost("Create")]
    public IActionResult Create([FromBody] Product product)
    {


        if (_productRepo.CreateProduct(product))
        {

            return Ok(true);
        }
        return Ok(false);
    }

}
