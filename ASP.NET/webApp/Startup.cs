using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using ConsoleApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;

namespace webApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            // services.AddDbContextPool<cardsContext>(options =>
            //     options.UseLazyLoadingProxies()
            //         .UseSqlServer("Data Source=(localhost)\\MSSQLLocalDB;Initial Catalog=cards;Integrated Security=True"));

            //services.AddDbContextPool<ContosoPetsContext>(options =>
            //    options.UseLazyLoadingProxies()
            //        .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContosoPets;Integrated Security=True"));
            //services.Add(new ServiceDescriptor(typeof(cardsContext), new cardsContext(Configuration.GetConnectionString("DefaultConnection"))));
            //services.AddTransient<MySqlConnection>(_ => new MySqlConnection(Configuration["ConnectionStrings:Default"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
