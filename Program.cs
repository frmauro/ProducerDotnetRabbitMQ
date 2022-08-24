using System.Text;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using RabbitMQ.Client;
// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

var logger = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Literate)
.CreateLogger();

logger.Information("Testando o envio de mensagens para uma Fila do RabbitMQ");

string? queueName = "purshaseok";
var payload = "{'id': '0003', 'description': 'Order 003'}";

logger.Information($"Queue = {queueName}");

try
{
    var factory = new ConnectionFactory()
    {
        HostName = "localhost"
    };
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();

    channel.QueueDeclare(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: Encoding.UTF8.GetBytes(payload));
        logger.Information(
            $"[Mensagem enviada] {payload}");

    logger.Information("Concluido o envio de mensagens");

    Console.ReadLine();

}
catch (Exception ex)
{
    logger.Error($"Exceção: {ex.GetType().FullName} | " +
                                 $"Mensagem: {ex.Message}");
}