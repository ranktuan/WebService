using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minvoice.webhook.data.services
{
    public class SystemConnection
    {
        // rabbit
        public string Host { get;  set; } = "localhost";
        public string Username { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string QueueName { get; set; } = "C";

        // mongoDB
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "WebHookDB";
        public string CollectionName { get; set; } = "DataRabbit";

        //Minio
        public string endpoint { get; set; } = "play.min.io";
        public string accessKey { get; set; } = "NQTuan";
        public string secretKey { get; set; } = "NQTuan123";

    }
}
