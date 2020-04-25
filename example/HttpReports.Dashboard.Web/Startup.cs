using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HttpReports.Dashboard.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCap(x =>
            {  
                x.UseMySql("DataBase=HttpReports;Data Source=localhost;User Id=root;Password=123456;");    
                
                x.UseRabbitMQ("localhost");

                x.UseDashboard();

            });


            services.AddHttpReports().UseMySqlStorage().UseGrpc(); 

            services.AddHttpReportsDashboard().UseMySqlStorage();
           
            services.AddControllersWithViews();
        }

   
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpReports();

            app.UseHttpReportsDashboard();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
