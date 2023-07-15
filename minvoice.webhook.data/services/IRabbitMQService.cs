using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minvoice.webhook.data.services
{
    public interface IRabbitMQService
    {
        Task SaveMessageDB(CancellationToken stoppingToken);
        Task ConnectMinio(CancellationToken stoppingToken);
        void Dispose();
        
    }
    
}
