using System.Text;
using NATS.Client;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Listening in on SBN Camera...");

var cfg = new Dictionary<string, string>() {
    {"host", "video-beta.sbncloud.com"},
    {"port", "4222"},
    {"token", "secret"},
    {"chan", "camera.>"},
    {"excl", "camera.hiknet.>"}
};

long msgNo = 0;
ConnectionFactory cf = new ConnectionFactory();
Options opts = ConnectionFactory.GetDefaultOptions();
opts.Url = String.Format("nats://{0}@{1}:{2}", cfg["token"], cfg["host"], cfg["port"]);

using (IConnection c = cf.CreateConnection(opts))
{

    IAsyncSubscription s_id = c.SubscribeAsync("camera.>",
        (sender, args) =>
        {
            if (args.Message.Subject.StartsWith("camera.hiknet."))
                return;
            var no = Interlocked.Increment(ref msgNo);
            var str = Encoding.UTF8.GetString(args.Message.Data);
            Console.WriteLine($"[#{no}] {str}");
        });

    Console.ReadLine();
}
Console.WriteLine("DONE on SBN Camera...");
