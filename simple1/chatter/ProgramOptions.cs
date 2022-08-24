using CommandLine;
using System.Text.RegularExpressions;

namespace Chatter
{
    public class ProgramOptions
    {

        string m_Name = "";
        [Option('n', "name", Required = false,
            HelpText = "The name of the client instance. Will listen for messages sent to client name directly")]
        public string Name
        {
            get => m_Name;
            set
            {
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                var str = rgx.Replace(value.ToUpper(), "");
                m_Name = str.Length > 1 ? str : m_Name;
            }
        }

        [Option('p', "port", Required = false,
            HelpText = "Nats message bus listening port. Defaults to 4222")]
        public int Port { get; set; } = 4222;

        public ProgramOptions()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            m_Name = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}