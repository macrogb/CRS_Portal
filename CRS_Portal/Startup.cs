using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CRS_Portal.Models;

namespace CRS_Portal
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
            LoadConnectionStrings();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                        
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddSessionStateTempDataProvider(); ;
            
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = 1073741824;
                options.MultipartBodyLengthLimit = 1073741824;
                options.MultipartHeadersLengthLimit = 1073741824;
            });
           

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            

                //app.UseHttpsRedirection();
                app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseSession();         

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });
            
        }

        private void LoadConnectionStrings()
        {
            //Temp
            //CRSMacroSettings.BankName = Configuration.GetSection("Data").GetSection("BankName").Value;
            CRSMacroSettings.Initial_CS = Configuration.GetSection("Data").GetSection("Initial_CS").Value;
            CRSMacroSettings.Canara_CS = Configuration.GetSection("Data").GetSection("Canara_CS").Value;
            CRSMacroSettings.BOI_CS = Configuration.GetSection("Data").GetSection("BOI_CS").Value;
            CRSMacroSettings.ICICI_CS = Configuration.GetSection("Data").GetSection("ICICI_CS").Value;

            CRSMacroSettings.Domain = Configuration.GetSection("Data").GetSection("Domain").Value;
            CRSMacroSettings.UserName = Configuration.GetSection("Data").GetSection("UserName").Value;
            CRSMacroSettings.Password = Configuration.GetSection("Data").GetSection("Password").Value;
            CRSMacroSettings.CRSWebAPIURL = Configuration.GetSection("Data").GetSection("CRSWebAPIURL").Value;
            CRSMacroSettings.LogFilePath = Configuration.GetSection("Data").GetSection("CRSPortalLogFilePath").Value;
            CRSMacroSettings.FilePath = Configuration.GetSection("Data").GetSection("CRSPortalFilePath").Value;
        }

    }
}
