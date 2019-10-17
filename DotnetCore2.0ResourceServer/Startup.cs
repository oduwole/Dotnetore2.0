using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotnetCore2.Infrastrucutre.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace DotnetCore2.ResourceServer
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
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            //services.AddTransient<IBoardsService, BoardsService>()
                //.AddTransient<IBoardsRepository, BoardsRepository>()
                //.AddTransient<IProductsRepository, ProductsRepository>()
                //.AddTransient<IProductsService, ProductsService>()
                //.AddTransient<IProductsImageService, ProductsImageService>()
                //.AddTransient<IProductsImageRepository, ProductsImageRepository>()
                //.AddTransient<ICategoryService, CategoryService>()
                //.AddTransient<ICategoryRepository, CategoryRepository>()
                //.AddTransient<IOrderService, OrderService>()
                //.AddTransient<IOrderRepository, OrderRepository>()
                //.AddTransient<ICartService, CartService>()
                //.AddTransient<ICartRepository, CartRepository>()
                //.AddTransient<IPaymentMethodsService, PaymentMethodsService>()
                //.AddTransient<IPaymentMethodsRepository, PaymentMethodsRepository>()
                //.AddTransient<ICurrencyService, CurrencyService>()
                //.AddTransient<ICurrencyRepository, CurrencyRepository>()

                //.AddTransient<IUserService, UserService>()
                //.AddTransient<IUserRepository, UserRepository>()
                //.AddTransient<IUserProfileRepository, UserProfileRepository>()
                //.AddTransient<IUserProfileService, UserProfileService>()
                ////.AddTransient<UserManager<AppUser>>()
                //.AddTransient<IUnitOfWork, UnitOfWork>();

            //services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = "http://localhost:8060";
                o.Audience = "vbIncAppApi";
                o.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiReader", policy => policy.RequireClaim("scope", "api.read"));
                options.AddPolicy("Consumer", policy => policy.RequireClaim(ClaimTypes.Role, "consumer"));
            });
            //.AddIdentity<IdentityUser, IdentityRole>(options =>
            //{
            //    options.User.RequireUniqueEmail = false;
            //});
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               //.AllowCredentials()
               .AllowAnyHeader()));
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "VBIncApp", Version = "v1" });
                // Swagger 2.+ support
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VBIncApp ResourceApi V1");
            });
            app.UseSwagger();
            app.UseAuthentication();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
