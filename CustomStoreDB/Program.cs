using ServicesContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IClothesService, ClothesService>();
builder.Services.AddSingleton<ICostumeService, CostumeService>();
builder.Services.AddSingleton<IProductsServices, ProductService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
