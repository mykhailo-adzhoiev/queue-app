using Microsoft.Extensions.DependencyInjection;
using QueueMessageApp.Worker.Services;
using System.Threading.Tasks;

namespace QueueMessageApp.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var startup = new Startup();
            var service = startup.Provider.GetRequiredService<IWorkerService>();
            await service.Run();
        }
    }
}
