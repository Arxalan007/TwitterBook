using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.Saml2P;
using TwitterBook.Installers;
using TwitterBook.Options;

namespace TwitterBook
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
            services.AddAuthentication(opt =>
                {
                    // Default scheme that maintains session is cookies.
                    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    // If there's a challenge to sign in, use the Saml2 scheme.
                    opt.DefaultChallengeScheme = Saml2Defaults.Scheme;
                }).AddCookie().AddSaml2(opt =>
                {
                    // Set up our EntityId, this is our application.
                    opt.SPOptions.EntityId = new EntityId("https://localhost:5001/Saml2");

                    // Single logout messages should be signed according to the SAML2 standard, so we need
                    // to add a certificate for our app to sign logout messages with to enable logout functionality.
                    opt.SPOptions.ServiceCertificates.Add(new X509Certificate2("Sustainsys.Saml2.Tests.pfx"));

                    // Add an identity provider.
                    opt.IdentityProviders.Add(new IdentityProvider(
                        // The identityprovider's entity id.
                        new EntityId("https://stubidp.sustainsys.com/Metadata"),
                        opt.SPOptions)
                    {
                        // Load config parameters from metadata, using the Entity Id as the metadata address.
                        LoadMetadata = true
                    });
                });
            services.InstallServicesInAssembly(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            var saml = new Saml2AuthenticationRequest();
            Configuration.GetSection(nameof(Saml2AuthenticationRequest)).Bind(saml);
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            
            app.UseSwagger(option =>
            {
                option.RouteTemplate = swaggerOptions.JsonRoute;
            });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}