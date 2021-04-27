using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CommonRedis;

namespace SubscriberWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);

            try
            {
                var redis = RedisStore.RedisCache;
                            
                //create a publisher
                var sub = redis.Multiplexer.GetSubscriber();       

                //sub to test channel a message
                await sub.SubscribeAsync("testChannel", (channel, message) => {
                    Console.WriteLine("Got notification: " + (string)message);
                    //do stuff
                    Console.Read();
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis Publish Exception: {ex.Message}");
            }

            }
        }
    }
}
