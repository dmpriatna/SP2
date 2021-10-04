using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using SP2.Data;

namespace SP2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public const string CorsPolicy = "AllowAny";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy(CorsPolicy, pol => pol
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
            });

            services.AddControllers();

            services.AddSwaggerGen(opt => opt
                .SwaggerDoc("SP2", new OpenApiInfo
                {
                    Title = "SP2.API",
                    Version = "1.0"
                }));
        
            services.AddDbContext<GoLogContext>(Setup);
        }

        private void Setup(DbContextOptionsBuilder obj)
        {
            var connectionString = Configuration.GetConnectionString("GoLog");
            obj.UseNpgsql(connectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(opt => 
            {
                opt.RoutePrefix = "";
                opt.SwaggerEndpoint("/swagger/SP2/swagger.json", "Version 1.0");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
