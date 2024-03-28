using UnitOfWorkTrial.Models;
using UnitOfWorkTrial.UOW;
using Microsoft.EntityFrameworkCore;


namespace CRUDinCoreMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Configure the ConnectionString and DbContext Class
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Registering the UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddSession();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //Setting the Employees and Index action method as the default Route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Employees}/{action=Index}/{id?}");    //Conventional Routing

            app.Run();
        }
    }
}