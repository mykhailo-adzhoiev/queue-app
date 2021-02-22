using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueueMessageApp.BL.Infrastructure;
using QueueMessageApp.BL.Messaging;
using QueueMessageApp.DAL.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QueueMessageApp.Worker.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IBusControl _busControl;

        public WorkerService(IServiceCollection services, IConfiguration configuration)
        {
            var messageRepository = services.BuildServiceProvider().GetRequiredService<IMessageRepository>();
            var rabbitMqConfiguration = configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>();

            _busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(rabbitMqConfiguration.Hostname, "/", h =>
                {
                    h.Username(rabbitMqConfiguration.UserName);
                    h.Password(rabbitMqConfiguration.Password);
                });

                cfg.ReceiveEndpoint(rabbitMqConfiguration.QueueName, endpointCfg =>
                {
                    endpointCfg.Consumer(() => new MessageCreatedConsumer(messageRepository));
                });
            });
        }

        public async Task Run()
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            try
            {
                await _busControl.StartAsync(source.Token);

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();

                _busControl.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await _busControl.StopAsync(source.Token);
            }
        }
    }
}
