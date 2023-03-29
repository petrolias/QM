using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QM.Core.Abstractions;
using QM.Core.IO;
using QM.DAL.Abstractions;
using QM.Models.Abstractions;
using QM.QueueMessage;
using System.Diagnostics;

namespace QM.Core
{

    public class RegistrationService : BackgroundService
    {
        //private readonly IMessageQueue _queue;
        private readonly IRepository _dbRepository;
        private readonly IFileIO _fileIO;
        private readonly ILogger<RegistrationService> _logger;

        public RegistrationService(
            //IMessageQueue queue, 
            IRepository dbRepository,
            IFileIO fileIO,
            ILogger<RegistrationService> logger)
        {
            // _queue = queue;
            this._dbRepository = dbRepository;
            this._fileIO = fileIO;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {                
                using (var bus = new MessageBus(queueName : "MockQueue"))
                {
                    // Subscribe to messages
                    bus.Subscribe<IMessage>(message =>
                    {
                        Console.WriteLine($"Received message: {message.Text}");
                        
                        //return Task.CompletedTask;
                    });

                    // Publish a message
                    //var message = new { Text = "Hello, world!" };
                    //await bus.Publish<IMessage>(message);

                    //Console.ReadLine();
                }

                // Receive the 'New Registration' event from the message queue
                var registrationEvent = await ReceiveEventAsync();

                // Persist the event in the database and file storage
                var persistTask = Task.WhenAll(
                    PersistInDatabaseAsync(registrationEvent),
                    PersistInFileAsync(registrationEvent));

                await persistTask;

                // Create an object with the extra properties
                var extraProperties = new
                {
                    PersistDateTime = DateTime.Now,
                    TotalPersistTime = persistTask.Result.Sum(result => result.TimeTaken),
                    SystemsPersistedTo = new[] { "Database", "File Storage" }
                };

                // Repost the event to the HTTP endpoints with the extra properties
                var repostTask = Task.WhenAll(
                    RepostToEndpointAsync(registrationEvent, "http://endpoint1.com", extraProperties),
                    RepostToEndpointAsync(registrationEvent, "http://endpoint2.com", extraProperties));

                await repostTask;

                // Log all actions
                _logger.LogInformation($"Registration event {registrationEvent.Id} processed. Persist time: {extraProperties.TotalPersistTime}. Repost status: {repostTask.Result[0].StatusCode}, {repostTask.Result[1].StatusCode}");

                // Wait for 5 seconds before processing the next event
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            var message = await _queue.ReceiveAsync(stoppingToken);

        //            // Parse the message into a NewRegistrationEvent object
        //            var registrationModel = JsonConvert.DeserializeObject<IRegistrationModel>(message.Body);

        //            // Persist the event in the DB and file storage
        //            var stopwatch = Stopwatch.StartNew();
        //            await _dbStorage.SaveAsync(registrationModel);
        //            await _fileIO.SaveAsync(registrationModel);
        //            stopwatch.Stop();

        //            // Log the persistence time and systems persisted to
        //            _logger.LogInformation($"Persisted new registration event for user {registrationModel.UserId} to DB and file storage. Total persist time: {stopwatch.ElapsedMilliseconds}ms");

        //            // Repost the event to the http endpoints
        //            //var repostResult1 = await HttpUtils.PostAsync("http://endpoint1.com", newRegistrationEvent);
        //            //var repostResult2 = await HttpUtils.PostAsync("http://endpoint2.com", newRegistrationEvent);

        //            // Log the repost results
        //            _logger.LogInformation($"Reposted new registration event for user {newRegistrationEvent.UserId} to http endpoints. Results: Endpoint1={repostResult1}, Endpoint2={repostResult2}");
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "An error occurred while processing a new registration event.");
        //        }
        //    }
        //}
    }
}