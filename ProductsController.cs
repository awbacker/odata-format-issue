using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api;

[Route("xyz/Products")]
public class ProductsController : ODataController
{
    protected DatabaseContext DbContext { get; }

    public ProductsController(DatabaseContext context)
    {
        DbContext = context;
    }

    [EnableQuery]
    [HttpGet] // [HttpGet("xyz/Products")]
    public IQueryable<Product> Get()
    {
        return DbContext.Products;
    }

    [EnableQuery]
    [HttpGet("{key}")] // [HttpGet("xyz/Products({key})")]
    public SingleResult<Product> Get(int key)
    {
        return SingleResult.Create(DbContext.Products.Where(o => o.Id == key));
    }
}