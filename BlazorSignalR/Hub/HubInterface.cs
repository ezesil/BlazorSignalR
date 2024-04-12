using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace BlazorSignalR.Hub
{
    public class HubInterface
    {      
        public static readonly ConcurrentDictionary<string, ClientReference> Clients = new ConcurrentDictionary<string, ClientReference>();
      
        public IHubContext<ClientHub> Hub { get; set; }

        public HubInterface(IHubContext<ClientHub> hub)
        {
            Hub = hub;
        }

        public async Task<int> GetSensor(string id)
        {         
            return await Hub.Clients.Client(id).InvokeAsync<int>("GetSensor", new CancellationToken());
        }

        public class ClientReference
        {
            public string Id { get; set; }
            public string? Name { get; set; }
            public string? Temperature { get; set; }

            public ClientReference()
            {

            }

            public ClientReference(string id, string? name = null)
            {
                Id = id;
                Name = name;
            }
        }
    }
}
