namespace WindowsServiceApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client= new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)

        {
            client.Dispose();
            _logger.LogInformation("The service has stopped");
            return base.StopAsync(cancellationToken);
        }

        //El objetivo de este método es verificar el estado de un sitio web cada 5 segundos,
        //y registrar en el registro de la aplicación en un archivo .txt si el sitio está activo o inactivo.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var randomNumber = new Random().NextDouble();
                if (randomNumber < 0.2)
                {
                    try
                    {
                        List<string> strings = new List<string> { "foo", "bar", "baz" };
                        string text = strings[3];
                    }

                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Simulated error occurred.");
                    }
                    await Task.Delay(5000, stoppingToken);
                    continue;
                }
                else
                {
                    _logger.LogError("Not Error.");
                }

                var result = await client.GetAsync("https://www.google.cl");

                if (result.IsSuccessStatusCode)
                {
                  _logger.LogInformation("The site is active.Status code {StatusCode}", result.StatusCode);
                }
                else
                {
                    _logger.LogError("The site is inactive.Status code {StatusCode}", result.StatusCode);
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}