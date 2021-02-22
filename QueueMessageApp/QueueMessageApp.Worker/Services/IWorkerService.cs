using System.Threading.Tasks;

namespace QueueMessageApp.Worker.Services
{
    public interface IWorkerService
    {
        Task Run();
    }
}
