using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IRO_UNMO.App.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IRO_UNMO.App.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using IRO_UNMO.App.Hubs;
using Microsoft.AspNetCore.SignalR;
using IRO_UNMO.App.Subscription;
using IRO_UNMO.App.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IRO_UNMO.App
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
            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("lokalni")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddRoles<IdentityRole>()
              .AddDefaultTokenProviders();


            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IMyUser, MyUser>();
            services.AddScoped<INotification, NotificationService>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();

            // dependency injection
            //services.AddSingleton<NotificationDatabaseSubscription, NotificationDatabaseSubscription>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalServer>("/signalServer");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "area",
                   template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseSqlTableDependency<NotificationDatabaseSubscription>(Configuration.GetConnectionString("lokalni"));
        }
    }
}
