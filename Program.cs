using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;
using webapplication2.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();


builder.Services.AddDbContext<BethanysPieShopDbContext>(options => {
    options.UseNpgsql(
        builder.Configuration["ConnectionStrings:BethanysPieShopDbContextConnection"]);
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

DbInitializer.Seed(app);


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


app.MapControllerRoute(
    name: "product_detail",
    pattern: "product/{id}",
    defaults: new { controller = "Product", action = "Detail" });


