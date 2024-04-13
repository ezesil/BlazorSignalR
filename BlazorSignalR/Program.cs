using BlazorSignalR.Components;
using BlazorSignalR.Hub;
using Microsoft.AspNetCore.SignalR;
using static System.Net.WebRequestMethods;

namespace BlazorSignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            bool.TryParse(Environment.GetEnvironmentVariable("USE_HTTPS"), out bool use_https);

            var protocol = use_https ? "https" : "http";
            var port = Environment.GetEnvironmentVariable("PORT") ?? "5001";
            var url = $"{protocol}://0.0.0.0:{port}";


            builder.WebHost.UseUrls($"{url}");

            // Agregar servicios a contenedor
            builder.Services.AddRazorComponents(); 

            builder.Services
                .AddServerSideBlazor()
                .AddInteractiveServerComponents();

            builder.Services.AddSignalR(); // Agregar SignalR al servicio de la aplicaci�n
            builder.Services.AddControllers(); // <-- Agrega esta l�nea

            var app = builder.Build();

            app.UseStaticFiles();

            // Mapear la aplicaci�n de Blazor Server
            app.MapRazorComponents<App>()
               .AddInteractiveServerRenderMode(); // Configurar Blazor Server

            app.UseRouting();

            // Agregar app.UseAntiforgery() entre app.UseRouting() y app.UseEndpoints(...)
            app.UseAntiforgery();

            app.UseAuthentication(); // Si est�s usando autenticaci�n
            app.UseAuthorization(); // Si est�s usando autorizaci�n

            // Configurar los endpoints de la aplicaci�n
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Mapear controladores (si es necesario)

                // Mapear el Hub de SignalR
                endpoints.MapHub<ClientHub>("/clienthub");
            });

            app.Run();
        }
    }
}
