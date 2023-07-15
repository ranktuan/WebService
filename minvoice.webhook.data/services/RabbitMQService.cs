using Minio;
using minvoice.webhook.data.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Text;

namespace minvoice.webhook.data.services
{
    public class RabbitMQService : IRabbitMQService
    {      
        private string NameQueue;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public RabbitMQService(IConfiguration configuration)
        {
            SystemConnection _connectDB = new SystemConnection();
            var factory = new ConnectionFactory()
            {
                HostName = _connectDB.Host,
                UserName = _connectDB.Username,
                Password = _connectDB.Password,

            };
            NameQueue = _connectDB.QueueName;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(NameQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public async Task SaveMessageDB(CancellationToken stoppingToken)
        {
            SystemConnection _connectDB = new SystemConnection();
            var connectionString = _connectDB.ConnectionString;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(_connectDB.DatabaseName);
            var collection = database.GetCollection<BsonDocument>(_connectDB.CollectionName);
            try
            {
                await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                Console.WriteLine("Connect success to MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection errors to MongoDB: " + ex.Message);
            }
            var channel = _connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                Message item = new Message();
                item.message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine("Receive message to RabbitMQ: " + item.message);
                BsonDocument document = new BsonDocument{
                    { "Code",item.message},
                };             
                try
                {
                    collection.InsertOne(document);
                    Console.WriteLine("Save Success!");
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {                  
                    Console.WriteLine($"Error saving message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }              
            };
            channel.BasicConsume(queue: NameQueue, autoAck: true, consumer: consumer);
            Console.WriteLine("Listening to messages from RabbitMQ...");
        }

        public async Task ConnectMinio(CancellationToken stoppingToken)
        {
            // dsfsdggs
            var a = "";
            SystemConnection _connectDB = new SystemConnection();
            try
            {
                MinioClient minioClient = new MinioClient()
                              .WithEndpoint(_connectDB.endpoint)
                              .WithCredentials(_connectDB.accessKey, _connectDB.secretKey)
                              .WithSSL()
                              .Build();
                Console.WriteLine("Connect success Minio {0} {1}" ,a);
            }
            catch
            {
                Console.WriteLine("Error success Minio");
            }

        }
        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }

    }
}