using Amazon.Util;
using minvoice.webhook.data;
using minvoice.webhook.data.services;
using MongoDB.Driver;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<RabbitMQService, RabbitMQService>();

       
        IConfiguration configuration = hostContext.Configuration;

        services.AddSingleton(configuration);

        services.AddSingleton<IRabbitMQService, RabbitMQService>();

        services.AddSingleton<ILoggerFactory, LoggerFactory>();

        services.AddHostedService<Worker>();
        

        
    })
    .Build();

await host.RunAsync();
