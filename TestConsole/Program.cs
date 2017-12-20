using System;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using Grpc.Core;
using Smith.Proto.ProtoSmith;

namespace TestConsole
{
    class Program
    {
        private static UserPrincipal _ident = UserPrincipal.Current;

        static void Main(string[] args)
        {
            Console.WriteLine($"Bonjour {_ident.GivenName}, je suis l'agent Smith. Posez votre question.");
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            var client = new Bot.BotClient(channel);
            uint id = 1;
            
            var exit = false;
            while (!exit)
            {
                Console.Write(":");

                var ask = Console.ReadLine();

                var response = await client.AskAsync(new AskRequest {Id = id++, Message = ask});

                foreach (var field in response.Answers)
                {
                    Console.WriteLine(field);
                }

                

            }
            
        }
    }
}
