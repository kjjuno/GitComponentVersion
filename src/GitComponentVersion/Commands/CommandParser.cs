using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitComponentVersion.CommandLine;

namespace GitComponentVersion.Commands
{
    /// <summary>
    /// Parses command line arguments and returns the appropriate implementation of <see cref="ICommand"/> for execution.
    /// </summary>
    public class CommandParser
    {
        /// <summary>
        /// Gets the implementation of <see cref="ICommand"/> that corresponds with the given arguments.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>An implementation of <see cref="ICommand"/> that matches the given arguments.</returns>
        public ICommand GetCommand(string[] args)
        {
            string invokedVerb = null;
            object invokedVerbInstance = null;

            if (!global::CommandLine.Parser.Default.ParseArguments(args, Options.Default,
                (verb, verbOptions) =>
                {
                    invokedVerb = verb;
                    invokedVerbInstance = verbOptions;
                }))
            {
                if (string.Equals(invokedVerb, "help", StringComparison.CurrentCultureIgnoreCase))
                {
                    Environment.Exit((int)ReturnCode.Success);
                }
                else
                {
                    Environment.Exit((int)ReturnCode.InvalidCommand);
                }
            }

            var options = invokedVerbInstance as CommonSubOptions;

            if (options != null)
            {
                var fileSystem = DependencyInjection.Resolve<IFileSystem>();

                options.Directory = string.IsNullOrEmpty(options.Directory)
                    ? Environment.CurrentDirectory
                    : fileSystem.Path.GetFullPath(options.Directory);
            }

            ICommand command = null;

            switch (invokedVerb)
            {
                case InitCommand.Name:
                    command = new InitCommand(options as InitSubOptions);
                    break;
            }

            return command;
        }
    }
}
