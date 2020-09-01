using AutoMapper;
using IRO_UNMO.WebAPI.Database;
using IRO_UNMO.Model.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IRO_UNMO.WebAPI.Services;
using Microsoft.AspNetCore.Authentication;

namespace IRO_UNMO.WebAPI
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
            services.AddMvc();

            #pragma warning restore CS0618 // Type or member is obsolete

            var connection = Configuration.GetConnectionString("LocalDB");

            services.AddDbContext<IRO_UNMO_Context>(options => options.UseSqlServer(connection));

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IApplicantService, ApplicantService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "IRO_UNMO API v1", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseAuthentication();

            //app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IRO_UNMO v1");
            });

            app.UseMvc();
        }
    }
}
