using ServicesContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IClothesService, ClothesService>();
builder.Services.AddScoped<ICostumeService, CostumeService>();
builder.Services.AddScoped<IProductsServices, ProductService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


builder.Services.AddDbContext<AllDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

builder.Host.ConfigureLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
