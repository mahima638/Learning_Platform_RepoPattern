using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Services;
using Microsoft.EntityFrameworkCore;
using LearningPlatformRepoPattern.Repository;

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
builder.Services.AddScoped<ITopicRepository, TopicService>();
builder.Services.AddScoped<IMaterialRepository, MaterialService>();

// Master Course
builder.Services.AddScoped<IMasterCourseRepository, MasterCourseService>();

// Sub Course
builder.Services.AddScoped<ISubCourseRepository, SubCourseService>();

// My Courses Repository and Service
builder.Services.AddScoped<IMyCoursesRepository, MyCoursesService>();

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