using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitComponentVersion.Busi;
using GitComponentVersion.CommandLine;

namespace GitComponentVersion.Commands
{
    /// <summary>
    /// An implementation of <see cref="ICommand"/> assists the user to create or modify their GitDepend.json file.
    /// </summary>
    public class InitCommand : ICommand
    {
        private readonly InitSubOptions _options;
        private readonly IConsole _console;

        /// <summary>
        /// The name of the verb.
        /// </summary>
        public const string Name = "init";

        /// <summary>
        /// Creates a new <see cref="InitCommand"/>
        /// </summary>
        /// <param name="options">The <see cref="InitSubOptions"/> that configures the command.</param>
        public InitCommand(InitSubOptions options)
        {
            _options = options;
            _console = DependencyInjection.Resolve<IConsole>();
        }

        #region Implementation of ICommand

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>The return code.</returns>
        public ReturnCode Execute()
        {
            return ReturnCode.Success;
        }

        #endregion
    }
}
