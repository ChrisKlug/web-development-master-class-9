using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebDevMasterClass.Services.Products.Data;
using WebDevMasterClass.Services.Products.Entities;

namespace WebDevMasterClass.Services.Products.Endpoints;

public class ProductEndpoint(IProducts products)
    : Endpoint<ProductEndpoint.Request, Results<Ok<Product>, NotFound>>
{
    public override void Configure()
    {
        Get("/api/products/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<Product>, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var product = await products.WithId(req.Id);
        return product is null 
            ? TypedResults.NotFound()
            : TypedResults.Ok(product);
    }

    public class Request
    {
        public int Id { get; set; }
    }
}