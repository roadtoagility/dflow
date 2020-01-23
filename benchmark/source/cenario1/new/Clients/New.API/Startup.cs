using System.Collections.Generic;
using Infraestructure.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductModule.Handlers;
using ProductModule.Queries;
using SharedKernel;

namespace New.API
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
            services.AddDbContext<BenchmarkDBContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("BdBenchmark"));
            });

            //IQueryHandler<TQuery,TResult>
            services.AddScoped<IQueryHandler<GetAllProducts, IEnumerable<Product>>  , GetAllProductsHandler>();    
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}