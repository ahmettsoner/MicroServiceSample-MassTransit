using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Product.Microservice.Context;

namespace Product.Microservice
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
            services.AddMassTransit(x=>{
                x.AddBus(p=> Bus.Factory.CreateUsingRabbitMq(c =>{
                    // c.UseHealthCheck(p);
                    c.Host(new Uri("rabbitmq://localhost"), h=>{
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product.Microservice", Version = "v1" });
            });

            services.AddDbContext<ApplicationContext>(o=> 
                o.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"),
                    b=> b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName))
                );
            services.AddMediatR(Assembly.GetEntryAssembly());
            services.AddScoped<IApplicationContext, ApplicationContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product.Microservice v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
