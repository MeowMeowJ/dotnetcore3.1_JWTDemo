using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Jwt.PolicyRequirement;
using Microsoft.AspNetCore.Authorization;
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
                // ������Ȩ�����
                // 1.���ڽ�ɫ��Ȩ
                options.AddPolicy("AdminPolicy1", options =>
                {
                    options.RequireRole("Admin").Build(); // Role
                });

                // 2. ��������
                options.AddPolicy("AdminClaim2", options =>
                {
                    //options.RequireClaim(ClaimTypes.Role, "Admin", "User").Build(); // ClaimType
                    //options.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "User").Build();
                    options.RequireClaim("laoliu", "liu").Build(); // ClaimType
                });

                //// 3. ��������/��ҪRequirement
                options.AddPolicy("AdminRequirement", options =>
                {
                    options.Requirements.Add(new AdminRequirement() { Name = "laoliu" }); // ��ȫ�Զ���
                });
            });

            services.AddSingleton<IAuthorizationHandler, MustRoleAdminHandler>();

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("laukitlaukitlaukit"));
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        // 3 + 2

                        // ��֤��Կ
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = securityKey,

                        // ��֤���
                        ValidateIssuer = true,
                        ValidIssuer = "issuer", // �ַ����������д����Ҫ�ͺ���һ��

                        // ��֤������
                        ValidateAudience = true,
                        ValidAudience = "audience",

                        // ��Ҫ��֤����ʱ��
                        RequireExpirationTime = true,
                        // ��������
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

            app.UseAuthentication(); // ������֤

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
