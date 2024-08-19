using GuraruRepository;
using GuraruRepository.Interface;
using GuraruRepository.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interface;
using Repository.Models;

var builder = WebApplication.CreateBuilder(args);

//PM> Scaffold-DbContext "Server=AYUSHLENOVO\SQLEXPRESS;Database=Guraru;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force
// Above script thre error so below is the modified script
// Scaffold-DbContext "Server=AYUSHLENOVO\SQLEXPRESS;Database=Guraru;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository<RawThreadDTO>>(m => new ThreadRepository<RawThreadDTO>("Server=AYUSHLENOVO\\SQLEXPRESS;Database=Guraru;Trusted_Connection=true"));
builder.Services.AddDbContext<DbContext, GuraruContext>(m => m.UseSqlServer("Server=AYUSHLENOVO\\SQLEXPRESS;Database=Guraru;Trusted_Connection=true"));
builder.Services.AddScoped<IGenericRepository<RawQuality>,GenericRepo<RawQuality>>();
//builder.Services.AddScoped<IGenericRepository<RawQuality>, GenericRepo<RawQuality>>();

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
