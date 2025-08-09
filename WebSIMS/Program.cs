using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebSIMS.DBContext;
using WebSIMS.Interfaces;
using WebSIMS.Services.Interfaces;
using WebSIMS.Repositories;
using WebSIMS.Services;



namespace WebSIMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure database with EntityFramework
            builder.Services.AddDbContext<SIMSDBContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register services (Dependency Inversion Principle)
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            
            // New services with SOLID principles
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<AutoEnrollmentService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IFacultyService, FacultyService>();

            // Configure Authentication
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("StudentOnly", policy => policy.RequireRole("Student"));
                options.AddPolicy("FacultyOnly", policy => policy.RequireRole("Faculty"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "faculty",
                pattern: "Faculty/{action=Index}/{id?}",
                defaults: new { controller = "Faculty", action = "Index" });

            app.Run();
        }
    }
}
