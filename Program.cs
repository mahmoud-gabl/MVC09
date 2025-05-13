using IKEA.BLL.Common.Services;
using IKEA.BLL.Common.Services.EmailSettings;
using IKEA.BLL.Services;
using IKEA.BLL.Services.Employee;
using IKEA.DAL.Models.Identity;
using IKEA.DAL.Presistance.Data;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.Repositories.Employees;
using IKEA.DAL.Presistance.UnitOfWork;
using IKEA.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace IKEA.PL

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region Configure Services
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>((optionsBuilder) =>
            {
                optionsBuilder.UseLazyLoadingProxies().
                UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IEmailSettings, EmailSettings>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>((options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = true;// @#$
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;

            }))
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();  // This line is essential to add the default token providers.;

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
            
            });
            #endregion


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
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
