using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Repositories;
using LearningPlatformRepoPattern.Repository;
using LearningPlatformRepoPattern.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Session
builder.Services.AddSession();

// ==========================================
// Repository + Service registrations
// ==========================================

// User
builder.Services.AddScoped<IUserRepository, UserService>();

// Master Course
builder.Services.AddScoped<IMasterCourseRepository, MasterCourseService>();

// Sub Course
builder.Services.AddScoped<ISubCourseRepository, SubCourseService>();

// My Courses
builder.Services.AddScoped<IMyCoursesRepository, MyCoursesRepository>();
builder.Services.AddScoped<IMyCoursesService, MyCoursesService>();

// Subscription
builder.Services.AddScoped<SubscriptionRepository>();
builder.Services.AddScoped<SubscriptionService>();


var app = builder.Build();

// ==========================================
// HTTP Pipeline
// ==========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();