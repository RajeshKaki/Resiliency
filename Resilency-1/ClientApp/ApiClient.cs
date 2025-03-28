using System.Diagnostics;

namespace ClientApp
{
    public class APIClient
    {
        private HttpClient _client;
        public APIClient(IHttpClientFactory httpClientfactory)
        {
            _client = httpClientfactory.CreateClient("RetryHandler");

            Console.WriteLine(_client.BaseAddress);
        }
        public void InvokeApi()
        {
            Stopwatch w = Stopwatch.StartNew();
            try
            {
                w.Start();

                var response = _client.GetAsync("http://localhost:5171/WeatherForecast").Result;
                Console.WriteLine(response.IsSuccessStatusCode);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                w.Stop();
                Console.WriteLine($"Total Time taken: {w.ElapsedMilliseconds}");
                w.Restart();
            }
        }

    }
}
