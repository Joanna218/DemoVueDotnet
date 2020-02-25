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

            // 使用預設的檔案作為進入點，並使其能使用靜態檔案作為網頁的資源
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use(async (context, next) => {
                await next();

                // 判斷是否要存取網頁，而不是發送API需求
                if (context.Response.StatusCode ==404 &&        // 該資源不存在
                    !System.IO.Path.HasExtension(context.Request.Path.Value) &&  // 網址最後沒有帶副檔名
                    !context.Request.Path.Value.StartsWith("/api"))              // 網址不是 /api 開頭
                {
                    context.Request.Path = "/index.html";       // 將網址改成 /index.html
                    context.Response.StatusCode = 200;          // 並將 HTTP 狀態碼修改為 200 成功
                    await next();
                }
            });
        }
    }
}
