using ABM.DAL.Repository;
using CA.BLL.Services;
using CA.DAL.Context;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Cryptovalue_Automation
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
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<ISymptomService, SymptomService>();
            services.AddScoped<ICryptocurrencyService, CryptocurrencyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICriteriaService, CriteriaService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddControllers();
            var conString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<EntityDbContext>(options => options.UseSqlServer(conString));

            services.AddHangfire(x => x.UseSqlServerStorage(conString));
            services.AddHangfireServer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cryptotrading_Automation", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cryptotrading_Automation v1"));
            }

            app.UseCors(builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(x => true)
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}