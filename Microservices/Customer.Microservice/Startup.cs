using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Customer.Microservice.Consumers;
using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Customer.Microservice
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
                x.AddConsumer<ProductConsumer>();
                x.AddBus(p=> Bus.Factory.CreateUsingRabbitMq(c =>{
                    // c.UseHealthCheck(p);
                    c.Host(new Uri("rabbitmq://localhost"), h=>{
                        h.Username("guest");
                        h.Password("guest");
                    });
                    c.ReceiveEndpoint("productQueue", q=>{
                        q.PrefetchCount = 20;
                        q.UseMessageRetry(r => {
                            r.Interval(2, 100);
                        });
                        q.ConfigureConsumer<ProductConsumer>(p);
                    });
                }));
                services.AddMassTransitHostedService();
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer.Microservice", Version = "v1" });
            });
            services.AddScoped<ConnectionConfiguration>(o=> new ConnectionConfiguration(Configuration.GetConnectionString("DefaultConnectionString")));
            services.AddMediatR(Assembly.GetEntryAssembly());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer.Microservice v1"));
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
