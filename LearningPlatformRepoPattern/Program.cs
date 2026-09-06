using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Interfaces;
using LearningPlatformRepoPattern.Services;
using Microsoft.EntityFrameworkCore;
using LearningPlatformRepoPattern.Repository;
using LearningPlatformRepoPattern.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

// User Repository
builder.Services.AddScoped<IUserRepository, UserService>();

builder.Services.AddScoped<IMasterCourseService, MasterCourseService>();
builder.Services.AddScoped<ISubCourseService, SubCourseService>();
// My Courses Repository and Service
builder.Services.AddScoped<IMyCoursesRepository, MyCoursesRepository>();
builder.Services.AddScoped<IMyCoursesService, MyCoursesService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
