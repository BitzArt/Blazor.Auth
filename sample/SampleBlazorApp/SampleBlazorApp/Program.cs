using BitzArt.Blazor.Auth;
using SampleBlazorApp.Components;
using SampleBlazorApp.Services;

namespace SampleBlazorApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddScoped<JwtService>();
        builder.Services.AddBlazorServerAuth<SampleServerSideAuthenticationService>();

        builder.Services.AddHttpClient();

        builder.Services.AddScoped(sp =>
            new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5034")
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(SampleBlazorApp.Client._Imports).Assembly);

        app.MapAuthEndpoints();

        app.Run();
    }
}