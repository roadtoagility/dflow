using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Infraestructure.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetMQ.Sockets;
using ProductModule.Handlers;
using ProductModule.Queries;
using SharedKernel;
using SharedKernel.Distribuited;


namespace New.API
{
    public class AspNetCoreDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public AspNetCoreDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Resolve(Type service)
        {
            return this._serviceProvider.GetService(service);
        }
    }
    
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
            services.AddDbContext<BenchmarkDBContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("BdBenchmark"));
            });

             var queryHandler = new QueryHandler(Configuration.GetValue<string>("EndpointServer:HandlerAddress"),
                 Configuration.GetValue<string>("EndpointServer:ExecutorAddress"));
            
            services.AddSingleton<ITransport<RouterSocket, DealerSocket>>(queryHandler);
            
            services.AddScoped<IDependencyResolver, AspNetCoreDependencyResolver>();
            services.AddScoped<IGetAllProductsHandler, GetAllProductsHandler>();
            
            services.AddScoped<IOutputTransport>(s => 
                new SharedKernel.Distribuited.ClientHandler(Configuration.GetValue<string>("EndpointServer:ExecutorAddress")));
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IHandler).IsAssignableFrom(p) && p.IsInterface).Skip(1).ToList();
            
            foreach (var type in types)
            {
                var handler = provider.GetService(type);
            }

            var queryHandler = provider.GetService<ITransport<RouterSocket, DealerSocket>>();
            queryHandler.Start();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}