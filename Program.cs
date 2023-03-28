using Api;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Conventions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DatabaseContext>();

// ----------------------------------------------------------------------------
// Odata configuration - only use metadata route
builder.Services.AddControllers().AddOData(options =>
{
    options.Conventions.Clear();
    options.Conventions.Add(new MetadataRoutingConvention());
    //options.Conventions.Add(new EntitySetRoutingConvention()); //  remove
    options.EnableQueryFeatures(maxTopValue: null);
    options.AddRouteComponents("xyz", ODataModel.GetEdmModel());
});

// ----------------------------------------------------------------------------
var app = builder.Build();
app.UseODataRouteDebug();

DatabaseContext.CreateOnStartup(app);

app.UseRouting();
app.MapControllers();
app.Run();