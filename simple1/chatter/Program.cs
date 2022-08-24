using System;
using System.Text;
using NATS.Client;
using CommandLine;
using System.Text.RegularExpressions;

namespace Chatter
{
    class Program
    {
        private static Random random = new Random();
        static string ChatterID(string[] args)
        {

            if (args.Length>0)
            {
                return args[0];
            }
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());

        }

        static (string target, string message) ParseInput(string inp)
        {
            var splt = inp.Split(new Char[]{':'} ,2);
            if (splt.Length > 1)
            {
                return (splt[0], splt[1]);
            }
            return ("all", inp);
        }


        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<ProgramOptions>(args)
                .WithParsed<ProgramOptions>(
                    opts => RunCommand(opts)
                );            
        }

        static void PrintPrompt(string? printString = null)
        {
            if (!string.IsNullOrEmpty(printString))
                Console.WriteLine(printString);
            Console.Write("INP> ");
        }

        /// <summary>
        /// Executes the command set by the program's command line arguments.
        /// </summary>
        /// <param name="options">The options that dictate the SIP command to execute.</param>
        static void RunCommand(ProgramOptions options)
        {
            Console.WriteLine($"Chatter: {options.Name}");
            ConnectionFactory cf = new ConnectionFactory();
            Options opts = ConnectionFactory.GetDefaultOptions();
            opts.Url = $"nats://localhost:{options.Port}";

            IConnection c = cf.CreateConnection(opts);

            EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
            {
                PrintPrompt($"All received {args.Message}");
            };

            IAsyncSubscription s_all = c.SubscribeAsync("chatter.all", h);
            IAsyncSubscription s_id = c.SubscribeAsync($"chatter.{options.Name}",
                (sender, args) => PrintPrompt($"I received {args.Message}")
            );

            while (true)
            {
                PrintPrompt($"Chatter {options.Name} listening...");
                // Thread.Sleep(TimeSpan.FromSeconds(5));
                var inp = Console.ReadLine();
                if (inp=="Q" || inp=="q" || inp=="exit")
                {
                    break;
                }
                if (!string.IsNullOrWhiteSpace(inp))
                {
                    var (targ, msg) = ParseInput(inp);
                    c.Publish($"chatter.{targ}", Encoding.UTF8.GetBytes(msg));
                }
            }
            c.Close();
        }
    }
}