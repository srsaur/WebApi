using System;
using WebApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AngularASPNETCore2WebApiAuth.Auth;
using WebApi.IServices;
using WebApi.Services;
using System.Threading.Tasks;

namespace WebApi
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<AppDbContext>(e => e.UseSqlite("Data Source=local.db"));
            //services.AddDbContext<AppDbContext>(e => e.UseSqlServer(Configuration.GetConnectionString("default")));


            services.AddDefaultIdentity<AppUser>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddCors(e => e.AddPolicy("All",
                 p => p.AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithOrigins("http://localhost:4200","http://192.168.43.247:94")
                       .AllowCredentials()
                 ));

            var _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123456789ABCDEFGHIJ"));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));


            services.Configure<JwtIssuerOptions>(e =>
            {
                e.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                e.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                e.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtConfigration =>
            {
                jwtConfigration.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                jwtConfigration.TokenValidationParameters = tokenValidationParameters;
                jwtConfigration.SaveToken = true;
                jwtConfigration.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = (context) =>
                    {
                        var accessToken = context.Request.Query["access_Token"];
                        var path = context.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatApp"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(e => e.AddPolicy("SubPolicy", cp => cp.RequireClaim("iss")));

            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddScoped<IConnectionManager, ConnectionManager>();

            services.AddScoped<INotificationHelper, NotificationHelper>();
            services.AddTransient<IMessageService, MessageService>();

            services.Configure<IISOptions>(option =>
            {

            });

            services.AddTransient<IFriendRequestService, FriendRequestService>();
            services.AddSignalR();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseForwardedHeaders();

            app.UseCors("All");

            app.UseSignalR(e => {
                e.MapHub<ChatHub>("/chatApp");
            });

            app.UseMvc();
        }
    }
}
