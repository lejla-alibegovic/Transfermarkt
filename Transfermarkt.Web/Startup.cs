using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transfermarkt.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transfermarkt.Web.Models;
using System.Threading.Tasks;
using Transfermarkt.Web.Hubs;
using Transfermarkt.Web.Services;
using AutoMapper;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.SignalR;


namespace Transfermarkt.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            
            services.AddSingleton<IImagesService, ImagesService>();
            services.AddScoped<IData<City>, Data<City>>();
            services.AddScoped<IData<Country>, Data<Country>>();
            services.AddScoped<IData<League>, Data<League>>();
            services.AddScoped<IData<Player>, Data<Player>>();
            services.AddScoped<IData<Stadium>, Data<Stadium>>();
            services.AddScoped<IData<Club>, Data<Club>>();
            services.AddScoped<IData<Coach>, Data<Coach>>();
            services.AddScoped<IData<Referee>, Data<Referee>>();
            services.AddScoped<IData<Position>, Data<Position>>();
            services.AddScoped<IData<PlayerPosition>, Data<PlayerPosition>>();
            services.AddScoped<IData<Match>, Data<Match>>();
            services.AddScoped<IData<CoachClub>, Data<CoachClub>>();
            services.AddScoped<IData<Contract>, Data<Contract>>();
            services.AddScoped<IData<RefereeMatch>, Data<RefereeMatch>>();
            services.AddScoped<IData<Foul>, Data<Foul>>();
            services.AddScoped<IData<Corner>, Data<Corner>>();
            services.AddScoped<IData<Goal>, Data<Goal>>();
            services.AddScoped<IData<Contract>, Data<Contract>>();
            services.AddScoped<IMatches, MatchesService>();


            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(typeof(Startup));
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddIdentity<User, Role>(config => { config.SignIn.RequireConfirmedEmail = false; })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings 
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings 
                options.User.RequireUniqueEmail = true;
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
           
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                   
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<Notification>("/notifications");
            });
           
            //CreateUserRoles(serviceProvider).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Member", "Admin" };

            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new Role(roleName));
                }
            }

            var poweruser = new User
            {
                UserName = Configuration["UserSettings:Username"],
                Email = Configuration["UserSettings:UserEmail"]
            };

            string userPWD = Configuration["UserSettings:UserPassword"];
            var _user = await UserManager.FindByEmailAsync(Configuration["UserSettings:AdminUserEmail"]);

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, roleNames[1]);
                    await UserManager.AddToRoleAsync(poweruser, roleNames[2]);
                }
            }
        }
    }
}
