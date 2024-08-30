using ItemService.EventProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ItemService.RabbitMqClient
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly string _fila;
        private readonly IConnection _connection;
        private IModel _channel;
        private IEventProcessor _eventProcessor;

        public RabbitMqSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = Int32.Parse(_configuration["RabbitMqPort"]),
            }.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _fila = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _fila, exchange: "trigger", routingKey: "");
            _eventProcessor = eventProcessor;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ModuleHandle, ea) =>
            {
                ReadOnlyMemory<byte> body = ea.Body;
                string? message = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.Process(message);
            };
            _channel.BasicConsume(queue: _fila, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
