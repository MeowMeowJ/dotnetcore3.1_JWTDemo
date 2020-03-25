using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Jwt
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
            services.AddControllers();

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("AdminOrUser", options =>
                //{
                //    options.RequireRole("Admin, User").Build();
                //});
                //options.AddPolicy("AdminOrUser", options => {
                //    options.RequireRole("Admin", "User").Build();
                //});
                options.AddPolicy("AdminAndUser", options => {
                    options.RequireRole("Admin").RequireRole("User").Build();
                });
            });

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("laukitlaukitlaukit"));
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        // 3 + 2
                        
                        // 验证密钥
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = securityKey,

                        // 验证搭建人
                        ValidateIssuer = true,
                        ValidIssuer = "issuer", // 字符串可以随便写，但要和后面一致

                        // 验证订阅人
                        ValidateAudience = true,
                        ValidAudience = "audience",

                        // 需要验证过期时间
                        RequireExpirationTime = true,
                        // 生命周期
                        ValidateLifetime = true,
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication(); // 开启验证

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
