using Microsoft.Extensions.Logging;
using TriviadorClient.Entities;

namespace TriviadorClient
{
    class Startup
    {
        public Startup() 
        {
            StartClient();
        }

        public void StartClient()
        {
            var client = CreateClient();

            client.AddPlayer("Popitik");
            client.AddPlayer("Simpldimpl");
            client.AddPlayer("Fortnite");
        }

        public static Client GetClient()
        {
            return CreateClient();
        }

        private static Client CreateClient()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            return new Client(loggerFactory.CreateLogger("LogLevel"));
        }
    }
}
