using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using QueueMessageApp.BL.Command;
using QueueMessageApp.BL.Infrastructure;
using QueueMessageApp.BL.Messaging;
using QueueMessageApp.BL.Query;
using QueueMessageApp.DAL;
using QueueMessageApp.DAL.Models;
using QueueMessageApp.DAL.Repository;
using System.Collections.Generic;
using System.Reflection;

namespace QueueMessageApp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<QueueMessageAppDbContext>(options => options.UseSqlServer(connection));

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Message Api",
                    Description = "A simple API to send messages",
                    Contact = new OpenApiContact
                    {
                        Name = "Mykhailo Adzhoiev",
                        Email = "michael.adz@outlook.com"
                    }
                });
            });

            services.AddHealthChecks();
            services.AddOptions();

            IConfigurationSection serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            var rabbitMqConfiguration = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();

            services.AddMassTransit(cfg =>
            {
                cfg.UsingRabbitMq((context, config) =>
                {
                    config.Host(rabbitMqConfiguration.Hostname, "/", h =>
                    {
                        h.Username(rabbitMqConfiguration.UserName);
                        h.Password(rabbitMqConfiguration.Password);
                    });

                    config.OverrideDefaultBusEndpointQueueName(rabbitMqConfiguration.QueueName);
                });
            });

            services.AddMassTransitHostedService();

            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());


            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IMessageRepository, MessageRepository>();

            services.AddSingleton<IMessageCreateSender, MessageCreateSender>();

            services.AddTransient<IRequestHandler<CreateMessageCommand, Message>, CreateMessageCommandHandler>();
            services.AddTransient<IRequestHandler<GetMessagesByProcessedFilterQuery, List<Message>>, GetMessagesByProcessedFilterQueryHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Message API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
