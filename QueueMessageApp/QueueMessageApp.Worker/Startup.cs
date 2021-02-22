using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueueMessageApp.DAL;
using QueueMessageApp.DAL.Repository;
using QueueMessageApp.Worker.Services;
using System;
using System.IO;

namespace QueueMessageApp.Worker
{
    public class Startup
    {
        public IServiceProvider Provider { get; }
        public IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(services);
            services.AddSingleton(Configuration);

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<QueueMessageAppDbContext>(options => options.UseSqlServer(connection));

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IWorkerService, WorkerService>();

            Provider = services.BuildServiceProvider();
        }
    }
}
