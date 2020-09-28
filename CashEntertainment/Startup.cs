using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CashEntertainment.Authentications;
using CashEntertainment.DataAccess;
using CashEntertainment.DB;
using CashEntertainment.Helper; 
using CashEntertainment.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CashEntertainment
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
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            // Configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<Models_AppSettings>(appSettingsSection);

            //Configure Connection String Using Dependencies Injection
            services.AddDbContext<UAT_CasinoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("UAT_Connection")));



            // Configure jwt authentication
            var appSettings = appSettingsSection.Get<Models_AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.JWTSecret);



          



            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //Update At 20200331 Added Issuer and Audience If not It Will Not Work
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = appSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,
                };
            });


            services.AddAuthentication(CustomAuthenticationDefaults.BasicAuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(CustomAuthenticationDefaults.BasicAuthenticationScheme, null);

            //services.AddAuthentication("BasicAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


            services.AddAuthorization(o =>
            {
                // 添加策略，使用时在方法上标注[Authorize(Policy ="AdminPolicy")]，就会验证请求token中的ClaimTypes.Role是否包含了Admin
                o.AddPolicy("AdminPolicy", o =>
                {
                    o.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    o.RequireRole("ADMIN").Build();
                });

                //只有User的策略
                o.AddPolicy("MemberPolicy", o =>
                {
                    o.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    o.RequireRole("MEMBER").Build();
                });

                //Twelve都可以访问的策略
                o.AddPolicy("TwelvePolicy", o =>
                {
                    o.AuthenticationSchemes.Add(CustomAuthenticationDefaults.BasicAuthenticationScheme);
                    o.RequireRole("TWELVE").Build();
                });


            });



            //Configure Swagger.IO For API Documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "QPIN API with ASP.NET Core 3.0",
                    Contact = new OpenApiContact()
                    {
                        Name = "EGG GAMING",
                        Url = new Uri("http://www.tdz.co.ir")
                    }
                });
                var securityJWTSchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                var securityBasicSchema = new OpenApiSecurityScheme
                {
                    Description = "Basic auth added to authorization header",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "basic",
                    Type = SecuritySchemeType.Http,
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
                };

                c.AddSecurityDefinition("Bearer", securityJWTSchema);
                c.AddSecurityDefinition("Basic", securityBasicSchema);

                var securityJWTRequirement = new OpenApiSecurityRequirement();
                var securityBasicRequirement = new OpenApiSecurityRequirement();
                securityJWTRequirement.Add(securityJWTSchema, new[] { "Bearer" });
                securityBasicRequirement.Add(securityBasicSchema, new List<string>());
                c.AddSecurityRequirement(securityJWTRequirement);
                c.AddSecurityRequirement(securityBasicRequirement);
            });

            //Declare DL HttpContextAccessor for getting User Identity (LoginID) From The Jwt Token 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Declare DL ActionContextAccessor for getting User Remote IP Address 
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();


            //Remove Default 400 Error in Model Sate so that the error can dispplay with our custom format
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //Configure Our Repository Using Dependencies Injection
            services.AddTransient<IRepo_User, Repo_User>();
            services.AddTransient<IRepo_Bank, Repo_Bank>();
            services.AddTransient<IRepo_Admin, Repo_Admin>();
            services.AddTransient<IRepo_Announcement, Repo_Announcement>();
            services.AddTransient<IRepo_Game, Repo_Game>();
            services.AddTransient<IRepo_Twelve, Repo_Twelve>();
            services.AddTransient<IRepo_Payment, Repo_Payment>();
            services.AddTransient<IRepo_Topup, Repo_Topup>();
            services.AddTransient<IRepo_Withdraw, Repo_Withdraw>();
            services.AddTransient<IRepo_Wallet, Repo_Wallet>();
            services.AddTransient<IRepo_ExchangeRate, Repo_ExchangeRate>();
            services.AddTransient<IRepo_Settings, Repo_Settings>();
            services.AddTransient<IRepo_Hierarchy, Repo_Hierarchy>();
            services.AddTransient<IRepo_Winlose, Repo_Winlose>();
            services.AddSingleton<JWT>();
            services.AddSingleton<Common>();
            services.AddSingleton<UploadImagesHelper>();
            services.AddSingleton<HttpService>();
            services.AddSingleton<Intergration>();
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



            ///Return Image URL using staticfiles 
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "announcement")),
                RequestPath = "/announcement"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banner")),
                RequestPath = "/banner"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "topup")),
                RequestPath = "/topup"
            });

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
 

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
