using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Api;

[Table("product")]
public class Product
{
    [Key, Column("id")] public long Id { get; set; }
    [Column("sku"), StringLength(100)] public string Sku { get; set; } = null!;
    [Column("name"), StringLength(500)] public string Name { get; set; } = null!;
}

public class ODataModel
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Product>("Products");
        return builder.GetEdmModel();
    }
}

public class DatabaseContext : DbContext
{
    public IConfiguration Config { get; }
    public virtual DbSet<Product> Products { get; set; } = null!;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration config) : base(options)
    {
        Config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Config.GetConnectionString("sequel-lite"));
    }


    public static void CreateOnStartup(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetService<DatabaseContext>()!;
        if (!ctx.Database.EnsureCreated()) return;

        ctx.Products.Add(new Product() { Name = "Ultra Lox", Sku = "TP-LOX-002A" });
        ctx.Products.Add(new Product() { Name = "Plain Bagel", Sku = "FD-PLNB01" });
        ctx.Products.Add(new Product() { Name = "Cream Cheese", Sku = "TP-CHZC9" });
        ctx.SaveChanges();
    }
}