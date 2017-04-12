using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitComponentVersion.Busi;
using GitComponentVersion.Commands;
using Microsoft.Practices.Unity;

namespace GitComponentVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            DependencyInjection.Container = new UnityContainer();
            UnityConfig.RegisterTypes(DependencyInjection.Container);

            var parser = new CommandParser();
            var command = parser.GetCommand(args);

            if (command == null)
            {
                Environment.ExitCode = (int)ReturnCode.InvalidCommand;
                Console.Error.WriteLine("Invalid Command!");
            }
            else
            {
                var code = command.Execute();

                if (code != ReturnCode.Success)
                {
                    var strings = DependencyInjection.Resolve<IUiStrings>();
                    var console = DependencyInjection.Resolve<IConsole>();

                    var key = code.GetResxKey();
                    var str = strings.GetString(key);
                    console.WriteLine(str);
                }

                Environment.ExitCode = (int)code;
            }
        }
    }
}
