using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SampleSSO
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
            services.AddMvc();

            //services.AddCors();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
            //.AddOpenIdConnect("oidc", options =>
            //{
            //    options.SignInScheme = "Cookies";

            //    //options.Authority = "https://identityserver-gh.azurewebsites.net/core/";
            //    options.Authority = "https://prod-identityserver.usaid-apps-east.p.azurewebsites.net/core/";
            //    options.ClientId = "9fc034a1-4815-469d-8a8f-f7499e05f621";
            //    //options.Authority = "https://disdev2-identity.usaid.gov/core/";
            //    //options.ClientId = "C8A30346-BDCD-4D7E-B560-0B50F0075076";
            //    options.RequireHttpsMetadata = false;
            //    options.CallbackPath = "/index";
            //    options.SaveTokens = true;
            //    options.Scope.Add("roles");
            //    //options.Scope.Add("upn");
            //});
            //.AddOpenIdConnect("oidc", options =>
            // {
            //     options.SignInScheme = "Cookies";

            //     //options.Authority = "https://prod-identityserver.usaid-apps-east.p.azurewebsites.net/core/";
            //     options.Authority = "https://identitydev-test.usaid.gov/";
            //     options.ClientId = "C8A30346-BDCD-4D7E-B560-0B50F0075076";
            //     options.RequireHttpsMetadata = false;
            //     options.CallbackPath = "/Home";
            //     options.SaveTokens = true;
            //     //options.Scope.Add("roles");
            //     //options.Scope.Add("upn");
            // });

            //IdentityServer
            //.AddOpenIdConnect("oidc", options =>
            //{
            //    options.SignInScheme = "Cookies";
            //    options.Authority = "https://identitydev.usaid.gov/core/";
            //    options.ClientId = "9fc034a1-4815-469d-8a8f-f7499e05f621";
            //    options.RequireHttpsMetadata = false;
            //    options.CallbackPath = "/Home";
            //    options.SaveTokens = true;
            //    options.Scope.Add("roles");
            //    //options.Scope.Add("upn");
            //});

            //OKTA
            //.AddOpenIdConnect("oidc", options =>
            //{
            //    options.SignInScheme = "Cookies";
            //    options.Authority = "https://usaid-eval.okta.com/oauth2/default";
            //    options.ClientId = "0oa1hn4cac4BZMxnx0h8";
            //    options.RequireHttpsMetadata = false;
            //    options.CallbackPath = "/Home";
            //    options.SaveTokens = true;
            //    //options.Scope.Add("roles");
            //    //options.Scope.Add("upn");
            //});
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = Configuration.GetSection("AuthenticationServer:SignInScheme").Get<string>();
                options.Authority = Configuration.GetSection("AuthenticationServer:Authority").Get<string>();
                options.ClientId = Configuration.GetSection("AuthenticationServer:ClientId").Get<string>();
                options.RequireHttpsMetadata = Configuration.GetSection("AuthenticationServer:RequireHttpsMetadata").Get<bool>();
                options.CallbackPath = Configuration.GetSection("AuthenticationServer:CallbackPath").Get<string>();
                options.SaveTokens = Configuration.GetSection("AuthenticationServer:SaveTokens").Get<bool>();
                options.ResponseType = "id_token";
                options.ClientSecret = "0Zv-RDnIc2e8O7wFwov0jVfLA85EqzMt_fS9fxGt";
                //options.Scope.Add("allow_get");
                //options.Scope.Add("allow_post");
                options.Scope.Add("openid");
                
                //options.Scope.Add("open_id");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            

            //app.UseCors(builder => builder
            //    .WithOrigins("http://localhost:4200")
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials());

            app.UseAuthentication();

            //app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
