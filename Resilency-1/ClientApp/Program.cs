using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Microsoft.Extensions.Logging;
using Polly.Telemetry;
namespace ClientApp
{
    internal class Program
    {
        public class DummyClass
        {
            public int Value { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            ServiceCollection services = new ServiceCollection();
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            services.AddHttpClient("RetryHandler")
                    .AddResilienceHandler("RetryHander1",
                    (resilienceBuilder) =>
                    {
                        resilienceBuilder.ConfigureTelemetry(factory);

                        // Retry Strategy configuration
                        resilienceBuilder.AddRetry(new HttpRetryStrategyOptions // Configures retry behavior
                        {
                            MaxRetryAttempts = 2, // Maximum retries before throwing an exception (default: 3)

                            Delay = TimeSpan.FromSeconds(2), // Delay between retries (default: varies by strategy)

                            BackoffType = DelayBackoffType.Linear, // Exponential backoff for increasing delays (default)

                            UseJitter = false, // Adds random jitter to delay for better distribution (default: false)

                            ShouldHandle = new PredicateBuilder<HttpResponseMessage>() // Defines exceptions to trigger retries
                            .Handle<Exception>()
                            .Handle<HttpRequestException>()
                            .HandleResult(response => !(response.IsSuccessStatusCode) && response.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
                            // Includes non-successful responses
                        });

                        // Timeout Strategy configuration
                        resilienceBuilder.AddTimeout(TimeSpan.FromSeconds(10)); // Sets a timeout limit for requests (throws TimeoutRejectedException)

                    }
                   );

            services.AddSingleton<APIClient>();
            var service = services.BuildServiceProvider();
            var api = service.GetRequiredService<APIClient>();

            api.InvokeApi();

        }
    }
}
