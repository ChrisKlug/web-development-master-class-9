using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebDevMasterClass.Services.Products.Client;

namespace WebDevMasterClass.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductsClient productsClient) : ControllerBase
{
    [HttpGet("featured")]
    public async Task<Ok<Product[]>> GetFeaturedProducts()
        => TypedResults.Ok(await productsClient.GetFeaturedProducts());

    [HttpGet("{id:int}")]
    public async Task<Results<Ok<Product>, NotFound>> GetProduct(int id)
    { 
        var product = await productsClient.GetProduct(id);

        return product is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(product);
    }
}
