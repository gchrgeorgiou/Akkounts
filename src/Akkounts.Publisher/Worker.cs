using System;
using System.Threading;
using System.Threading.Tasks;
using Akkounts.Publisher.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Akkounts.Publisher
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private RestClient _restClient;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _restClient = new RestClient("http://localhost:5000");//This was https://akkounts.azurewebsites.net/ This is from where the transactions are sent, but I can't make it work
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var request = new RestRequest("Account");
                request.AddJsonBody(new RandomTransaction());
                _restClient.Post(request);

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}