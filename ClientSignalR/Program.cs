using Microsoft.AspNetCore.SignalR.Client;
using Shared.Tools;
using System.Security.Cryptography;

namespace ClientSignalR
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var hubConnection = new HubConnectionBuilder()
             .WithUrl("https://localhost:7228/clienthub") // Reemplaza esta URL con la URL de tu servidor SignalR
             .WithAutomaticReconnect()
             .Build();

            var nombre = RandomNameGenerator.GenerateRandomName();

            hubConnection.On("GetSensor", () => { return new Random().Next(-10, 50); });
            hubConnection.Reconnected += (string obj) =>
            {
                return hubConnection.SendAsync("SetName", nombre);
            };

            await hubConnection.StartAsync();
            Console.WriteLine("Conectado al servidor SignalR. Seteando nombre...");


            await hubConnection.SendAsync("SetName", nombre);
            Console.WriteLine($"Nombre del cliente seteado: {nombre}");


            Console.WriteLine("Presione ENTER para cerrar.");

            Console.ReadLine();

            await hubConnection.StopAsync();
        }
    }
}
