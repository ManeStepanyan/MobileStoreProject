﻿using System.IO;
using CatalogAPI.Models;
using DatabaseAccess.Repository;
using DatabaseAccess.SpExecuters;
using DatabaseAccessor.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogAPI
{
    public class Startup
    {
     /*   public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        } */

        // public IConfiguration Configuration { get; }
        private IConfiguration Configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {  // adding MVC Core,authorization and JSON formatting
            services.AddMvcCore()
                    .AddRazorViewEngine()
                    .AddAuthorization()
                    .AddJsonFormatters();

            // adding authentication info
            services.AddAuthentication("Bearer")
                   .AddIdentityServerAuthentication(options =>
                   {
                       options.Authority = "http://localhost:5000";
                       options.RequireHttpsMetadata = false;
                       options.ApiName = "CatalogAPI";
                   });
            // adding policies
            services.AddAuthorization(options => options.AddPolicy("Admin", policy => policy.RequireClaim("role", "1")));
            services.AddAuthorization(options => options.AddPolicy("Seller", policy => policy.RequireClaim("role", "2")));
            services.AddAuthorization(options => options.AddPolicy("Customer", policy => policy.RequireClaim("role", "3")));
            services.AddAuthorization(options => options.AddPolicy("Admin, Seller", policy => policy.RequireClaim("role", "1", "2")));
            services.AddAuthorization(options => options.AddPolicy("Admin, Customer", policy => policy.RequireClaim("role", "1", "3")));
            // adding singletons
                services.AddSingleton(new Repo<SellerProduct>(
               new MapInfo(this.Configuration["Mappers:Catalog"]),
               new SpExecuter(this.Configuration["ConnectionStrings:CatalogDB"])));
            services.AddSingleton(new Repo<CustomerOrder>(
            new MapInfo(this.Configuration["Mappers:Catalog"]),
            new SpExecuter(this.Configuration["ConnectionStrings:CatalogDB"])));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();

        }

    }
}
