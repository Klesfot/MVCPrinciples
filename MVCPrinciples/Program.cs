using Microsoft.EntityFrameworkCore;
using MVCPrinciples.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
var section = builder.Configuration.GetSection(Options.SectionName);
builder.Services.Configure<Options>(section);

// Register db contexts to the DI container
var connectionString = builder.Configuration["CustomAppConfig:DbConnectionString"];

builder.Services.AddDbContext<CategoryContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ProductContext>(options => options
    .UseSqlServer(connectionString));

builder.Services.AddDbContext<SupplierContext>(options => options
    .UseSqlServer(connectionString));

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
