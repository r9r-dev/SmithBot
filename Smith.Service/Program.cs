using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Cognitive.LUIS;
using Smith.Proto.ProtoSmith;

namespace Smith.Service
{
    class Program
    {
        private const int PORT = 50051;

        static void Main(string[] args)
        {
            using (var luisClient = new LuisClient("1a2563a4-7c29-46d5-86d4-3cd246288f02",
                "a03d446c9109446caec66ee99b58589a",
                true, "westeurope"))
            {
                var server = new Server
                {
                    Services = { Bot.BindService(new BotService(luisClient)) },
                    Ports = { new ServerPort("localhost", PORT, ServerCredentials.Insecure) }
                };

                server.Start();

                Console.WriteLine("Server listening on port " + PORT);
                Console.WriteLine("Press any key to stop the server...");
                Console.ReadKey();

                server.ShutdownAsync().Wait();
            }
        }
    }
}
