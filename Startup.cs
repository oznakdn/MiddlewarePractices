using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MiddlewarePractices.Middlewares;

namespace MiddlewarePractices
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiddlewarePractices", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /* Middleware
         * Request ile response arasındaki işlemleri sıralamayı sağlar.
         * Pipeline dır.
         * Asenkron olarak çalışırlar.
         * app.Run() metodu, kendinden sonraki middleware'lere kısa devre yaptırır. Yani Run'dan sonraki akış çalışmaz.
         * app.Use() metodu, kendi işlemini yaptıktan sonra next metoduyla bir sonrakine aktarır ve onu çalıştırır.
         * app.Map() metodu, route'a göre middleware'leri yönetmemizi sağlar.
         * app.MapWhen() metodu, request'in içindeki herhangi bir parametreye göre tetiklenir.
        */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiddlewarePractices v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            /****************** Middleware metotları******************/

            //app.Run(async xontext=>Console.WriteLine("1"));
           // app.Run(async context=>Console.WriteLine("2")); =====> Çalışmaz

        //    app.Use(async (context,next)=>{
        //        Console.WriteLine("Middleware 1 başladı.");
        //        await next.Invoke();
        //        Console.WriteLine("Middleware 1 sonlandırılıyor.");
               
        //    });

        //    app.Use(async (context,next)=>{
        //        Console.WriteLine("Middleware 2 başladı.");
        //        await next.Invoke();
        //        Console.WriteLine("Middleware 2 sonlandırılıyor.");
               
        //    });

        //    app.Use(async (context,next)=>{
        //        Console.WriteLine("Middleware 3 başladı.");
        //        await next.Invoke();
        //        Console.WriteLine("Middleware 3 sonlandırılıyor.");
               
        //    });

           app.Use(async (context,next)=>{
               Console.WriteLine("Use middleware tetiklendi.");
               await next.Invoke();
               
           });

           app.Map("/example",internalapp=>   //example route'una bir istek gelirse çalışır.
           internalapp.Run(async context=>{
               Console.WriteLine("/example middleware tetiklendi.");
               await context.Response.WriteAsync("/example middleware tetiklendi.");
           }));   

           app.MapWhen(x=>x.Request.Method=="GET", internalapp=>{
                internalapp.Run(async context=>{
                Console.WriteLine("MapWhen middleware tetiklendi");
                await context.Response.WriteAsync("MapWhen middleware tetiklendi"); 
                });
           });

           // Custom middleware oluşturmak
           app.UseHello();                      

          

            /***********************************************************/


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           
        }
    }
}
