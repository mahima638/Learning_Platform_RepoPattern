using LearnigAppMVCCore.Data;
using LearnigAppMVCCore.Interfaces;
using LearnigAppMVCCore.Repositories;
using LearnigAppMVCCore.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);





// Add MVC services
builder.Services.AddControllersWithViews();

// Register Entity Framework Core
builder.Services.AddDbContext<SubscriptionContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("db")
    ));

// Register Repository and Service
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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