using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Services;
using Microsoft.EntityFrameworkCore;
using LearningPlatformRepoPattern.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

// User Repository
builder.Services.AddScoped<IUserRepository, UserService>();

// Master Course
builder.Services.AddScoped<IMasterCourseRepository, MasterCourseService>();

// Sub Course
builder.Services.AddScoped<ISubCourseRepository, SubCourseService>();

// My Courses Repository and Service
builder.Services.AddScoped<IMyCoursesRepository, MyCoursesService>();

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