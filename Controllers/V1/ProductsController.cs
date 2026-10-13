using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace VersioningDemo.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var product = new
        {
            id = 1,
            name = "Laptop",
            price = 25000
        };
        
        return Ok(product);
    }
}