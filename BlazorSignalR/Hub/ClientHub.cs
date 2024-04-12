namespace BlazorSignalR.Hub
{
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Concurrent;
    using static BlazorSignalR.Hub.HubInterface;

    public class ClientHub : Hub
    {
        public static event EventHandler ClientsChanged;

        public override Task OnConnectedAsync()
        {
            HubInterface.Clients.TryAdd(Context.ConnectionId, new ClientReference(Context.ConnectionId));
            ClientsChanged?.Invoke(this, EventArgs.Empty);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            HubInterface.Clients.TryRemove(Context.ConnectionId, out _);
            ClientsChanged?.Invoke(this, EventArgs.Empty);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SetName(string name)
        {
            try
            {
                var reference = HubInterface.Clients[Context.ConnectionId];
                reference.Name = name;
                ClientsChanged?.Invoke(this, EventArgs.Empty);
            }
            catch 
            {
                Context.Abort();
                ClientsChanged?.Invoke(this, EventArgs.Empty);
                Console.WriteLine($"No se encontró el cliente. Id usado: {Context.ConnectionId}");
            }
        }
    }
}
