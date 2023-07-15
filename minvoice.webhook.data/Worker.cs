
using minvoice.webhook.data.services;

namespace minvoice.webhook.data
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitMQService _rabbitMQService;
        public Worker(ILogger<Worker> logger, IRabbitMQService rabbitMQService)
        {
            _logger = logger;
            _rabbitMQService = rabbitMQService;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _rabbitMQService.SaveMessageDB(stoppingToken);
            _rabbitMQService.ConnectMinio(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}