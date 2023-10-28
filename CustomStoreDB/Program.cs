using ServiceContracts;
using Services;
using ServicesContracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IClothesService, ClothesService>();
builder.Services.AddScoped<ICostumeService, CostumeService>();
builder.Services.AddScoped<IProductsServices, ProductService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
