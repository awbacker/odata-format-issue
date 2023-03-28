# Simple OData Formatting Issue

The `ProductsController` routes are not being recognized as OData routes
when the `EntitySetRoutingConvention`. Because of this, the output
format does not include the necessary `"@odata.context": "..."` and
`"@odata.nextLink": "..."` properties.

* A local SQLite database is created on run
* OData lives at /xyz/*

```cs
builder.Services.AddControllers().AddOData(options =>
{
    ...
    options.AddRouteComponents("xyz", ODataModel.GetEdmModel());
});
```

Product lives at that route
```cs
[Route("xyz/Products")]
public class ProductsController : ODataController
{

    [EnableQuery, HttpGet]
    public IQueryable<Product> Get()
    {
        return DbContext.Products;
    }
```


## Running

* dotnet run
* https://localhost:7277/$odata

## Example Output

#### OData Endpoint Mappings

| Controller & Action                   | HttpMethods | Template      | IsConventional |
|---------------------------------------|-------------|---------------|----------------|
| MetadataController.GetMetadata        | GET         | xyz/$metadata | Yes            |
| MetadataController.GetServiceDocument | GET         | xyz           | Yes            |

#### Non-OData Endpoint Mappings

| Controller & Action              | HttpMethods | Template           | 
|----------------------------------|-------------|--------------------|
| Api.ProductsController.Get (Api) | GET         | xyz/Products       | 
| Api.ProductsController.Get (Api) | GET         | xyz/Products/{key} | 

