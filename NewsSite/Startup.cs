using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsSite.Data;

namespace NewsSite
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("HiddenNews", policy =>
                    policy.RequireRole("Administrator", "Publisher", "Subscriber"));
                options.AddPolicy("isOfAge", policy =>
                    policy.RequireClaim("MinimumAge"));
                options.AddPolicy("canPublishSport", policy =>
                    policy.RequireClaim("Publish", "sport", "all"));
                options.AddPolicy("canPublishEconomy", policy =>
                    policy.RequireClaim("Publish", "economy", "all"));
                options.AddPolicy("canPublishCulture", policy =>
                    policy.RequireClaim("Publish", "culture", "all"));
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseDirectoryBrowser();

            app.UseMvc();
        }
    }
}
