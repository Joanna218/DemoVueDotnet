using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DemoVueDotnet
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

            app.UseAuthorization();

            // �ϥιw�]���ɮק@���i�J�I�A�èϨ��ϥ��R�A�ɮק@���������귽
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use(async (context, next) => {
                await next();

                // �P�_�O�_�n�s�������A�Ӥ��O�o�eAPI�ݨD
                if (context.Response.StatusCode ==404 &&        // �Ӹ귽���s�b
                    !System.IO.Path.HasExtension(context.Request.Path.Value) &&  // ���}�̫�S���a���ɦW
                    !context.Request.Path.Value.StartsWith("/api"))              // ���}���O /api �}�Y
                {
                    context.Request.Path = "/index.html";       // �N���}�令 /index.html
                    context.Response.StatusCode = 200;          // �ñN HTTP ���A�X�קאּ 200 ���\
                    await next();
                }
            });
        }
    }
}
