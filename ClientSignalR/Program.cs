using Microsoft.AspNetCore.SignalR.Client;
using Shared.Tools;
using System.Security.Cryptography;

namespace ClientSignalR
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var url = Environment.GetEnvironmentVariable("HOST_URL");

            var hubConnection = new HubConnectionBuilder()
             .WithUrl($"{url}/clienthub") // Reemplaza esta URL con la URL de tu servidor SignalR
             .WithAutomaticReconnect()
             .Build();

            var nombre = RandomNameGenerator.GenerateRandomName();

            hubConnection.On("GetSensor", () => { return new Random().Next(-10, 50); });
            hubConnection.Reconnected += (string obj) =>
            {
                return hubConnection.SendAsync("SetName", nombre);
            };

            while (hubConnection.ConnectionId == null)
            {
                Console.WriteLine($"Intentando conectar al servidor de signalR en {url}...");
                hubConnection.StartAsync();
                await Task.Delay(2000);
            }
            Console.WriteLine("Conectado al servidor SignalR. Seteando nombre...");


            await hubConnection.SendAsync("SetName", nombre);
            Console.WriteLine($"Nombre del cliente seteado: {nombre}");


            Console.WriteLine("Presione ENTER para cerrar.");

            Console.ReadLine();

            await hubConnection.StopAsync();
        }
    }
}
