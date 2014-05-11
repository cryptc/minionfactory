using Common.JetBrainsAnnotations;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;

// NOTE:  https://github.com/NancyFx/Nancy/wiki/Hosting-nancy-with-owin#katana---httplistener-selfhost
namespace NancyServer
{
    public class Program
    {
        // NOTE: POWERSHELL COMMAND ->   & "C:\src\branches\MinionFactory\NancyServer\bin\debug\nancyserver.exe"

        public static void Main([CanBeNull] string[] args)
        {
            var builder = new UserArgumentBuilder();
            UserArgs userArgs = builder.BuildUserArgs(args);

            if (userArgs.Message.IsNotNullOrEmpty())
            {
                Console.Write(userArgs.Message);
                return;
            }

            var customNancyServer = new CustomNancyServer(userArgs);
            customNancyServer.Start();
        }
    }

    internal class UserArgumentBuilder
    {
        [NotNull]
        public UserArgs BuildUserArgs([CanBeNull] string[] args)
        {
            var userArgs = new UserArgs();

            if (args.IsNullOrEmpty())
                return userArgs;

            string firstArgument = args[0];

            if (firstArgument.Compare("/?") == 0
                || firstArgument.Compare("--help", StringComparison.OrdinalIgnoreCase) == 0)
            {
                userArgs.Message = @"
::HELP::
When calling 'nancyserver.exe' without any parameters, the defaults will be used to start a new server instance.
You may specify a port number:
    e.g.  ./nancyserver.exe 1234
To start the server under https, add '/s':
    e.g.  ./nancyserver.exe /s
    e.g.  ./nancyserver.exe 1234 /s
::Valid parameters::
/? or --help   To see this menu
/s             Start an instance of the server in HTTPS
0-65535        Port number for the server

";
                return userArgs;
            }

            foreach (var arg in args)
            {
                ushort portNumber;
                if (ushort.TryParse(arg, out portNumber))
                    userArgs.PortNumber = portNumber;

                if (arg.Compare("/s", StringComparison.OrdinalIgnoreCase) == 0)
                    userArgs.UseHttps = true;
            }

            return userArgs;
        }
    }

    internal class UserArgs
    {
        [CanBeNull]
        public string Message { get; set; }
        public ushort? PortNumber { get; set; }
        public bool UseHttps { get; set; }
    }

    internal class CustomNancyServer
    {
        public CustomNancyServer(bool useHttps=false)
        {
            string port = ConfigurationManager.AppSettings["port"];

            ushort portNumber;
            if (!ushort.TryParse(port, out portNumber))
                throw new ArgumentNullException("AppConfig value for 'port' does not contain a valid port number.");

            this.PortNumber = portNumber;
            this.IsHttps = useHttps;
        }
        
        public CustomNancyServer(ushort portNumber, bool useHttps=false)
        {
            this.PortNumber = portNumber;
            this.IsHttps = useHttps;
        }

        public CustomNancyServer([NotNull] UserArgs userArgs)
            : this(userArgs.UseHttps)
        {
            if (userArgs.PortNumber.HasValue)
                this.PortNumber = userArgs.PortNumber.Value;
        }

        public ushort PortNumber { get; private set; }
        public bool IsHttps { get; private set; }

        [NotNull]
        public string Url { get { return "http{0}://localhost:{1}".Fs(((this.IsHttps) ? "s" : ""), this.PortNumber); } }

        public void Start()
        {
            using (WebApp.Start<Startup>(this.Url))
            {
                Console.WriteLine("Running on {0}".Fs(this.Url));
                Console.WriteLine("Type 'exit' or '(q)uit' to exit/quit.");

                do
                {
                    string userInput = Console.ReadLine();
                    if (userInput.IsNotNullOrEmpty() && userInput.ContainsAny(new[] { "q", "quit", "exit" }))
                        break;
                }
                while (true);
            }
        }
    }

    internal class Startup
    {
        public void Configuration([NotNull] IAppBuilder app)
        {
            app.UseNancy();
        }
    }

}
