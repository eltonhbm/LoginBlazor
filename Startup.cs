using LoginBlazor.Authentication;
using LoginBlazor.Data;
using LoginBlazor.Data.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace LoginBlazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<IUserService, InMemoryUserService>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SistemasBr", a =>
                    a.RequireAuthenticatedUser().RequireClaim("Dominio", "sistemasbr"));

                options.AddPolicy("Parceiro", a =>
                    a.RequireAuthenticatedUser().RequireClaim("Dominio", "parceiro"));

                options.AddPolicy("Admin", a =>
                    a.RequireAuthenticatedUser().RequireClaim(ClaimTypes.Role, "admin"));

                options.AddPolicy("Suporte", a =>
                    a.RequireAuthenticatedUser().RequireClaim(ClaimTypes.Role, "suporte"));

                options.AddPolicy("Vendedor", a =>
                    a.RequireAuthenticatedUser().RequireClaim(ClaimTypes.Role, "vendedor"));

                options.AddPolicy("NivelSeguranca1", p =>
                    p.RequireAuthenticatedUser().RequireAssertion(context =>
                    {
                        var nivelClaim = context.User.FindFirst(claim => claim.Type.Equals("NivelSeguranca"));
                        if (nivelClaim == null) return false;
                        return int.Parse(nivelClaim.Value) >= 1;
                    }));

                options.AddPolicy("NivelSeguranca2", p =>
                    p.RequireAuthenticatedUser().RequireAssertion(context =>
                    {
                        var nivelClaim = context.User.FindFirst(claim => claim.Type.Equals("NivelSeguranca"));
                        if (nivelClaim == null) return false;
                        return int.Parse(nivelClaim.Value) >= 2;
                    }));

                options.AddPolicy("NivelSeguranca3", p =>
                    p.RequireAuthenticatedUser().RequireAssertion(context =>
                    {
                        var nivelClaim = context.User.FindFirst(claim => claim.Type.Equals("NivelSeguranca"));
                        if (nivelClaim == null) return false;
                        return int.Parse(nivelClaim.Value) >= 3;
                    }));

                options.AddPolicy("NivelSeguranca4", p =>
                    p.RequireAuthenticatedUser().RequireAssertion(context =>
                    {
                        var nivelClaim = context.User.FindFirst(claim => claim.Type.Equals("NivelSeguranca"));
                        if (nivelClaim == null) return false;
                        return int.Parse(nivelClaim.Value) >= 4;
                    }));

                options.AddPolicy("NivelSeguranca5", p =>
                    p.RequireAuthenticatedUser().RequireAssertion(context =>
                    {
                        var nivelClaim = context.User.FindFirst(claim => claim.Type.Equals("NivelSeguranca"));
                        if (nivelClaim == null) return false;
                        return int.Parse(nivelClaim.Value) >= 5;
                    }));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}