using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GitComponentVersion.Busi;
using GitComponentVersion.CommandLine;
using GitComponentVersion.Configuration;
using GitComponentVersion.Resources;

namespace GitComponentVersion.Commands
{
    /// <summary>
    /// An implementation of <see cref="ICommand"/> assists the user to create or modify their GitDepend.json file.
    /// </summary>
    public class InitCommand : ICommand
    {
        private readonly InitSubOptions _options;
        private readonly IFileSystem _fileSystem;
        private readonly IConsole _console;
        private readonly IGitComponentVersionFileFactory _factory;

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
            _fileSystem = DependencyInjection.Resolve<IFileSystem>();
            _console = DependencyInjection.Resolve<IConsole>();
            _factory = DependencyInjection.Resolve<IGitComponentVersionFileFactory>();
        }

        #region Implementation of ICommand

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>The return code.</returns>
        public ReturnCode Execute()
        {
            string dir;
            ReturnCode code;
            var config = _factory.LoadFromDirectory(_options.Directory, out dir, out code) ?? new GitComponentVersionFile();

            if (!config.Components.Any())
            {
                _console.WriteLine(strings.LETS_ADD_COMPONENTS);
            }
            else
            {
                _console.WriteLine(strings.COMPONENTS_SO_FAR);

                foreach (var component in config.Components)
                {
                    WriteLine($"<*>{component.Name}<*>", ConsoleColor.Green);
                }

                _console.WriteLine();
                _console.WriteLine(strings.YOU_CAN_ADD_MORE);
            }

            while(true)
            {
                _console.WriteLine(strings.PRESS_ENTER_TO_FINISH);
                _console.WriteLine();

                _console.WriteLine(strings.WHAT_IS_THE_COMPONENT_NAME);
                _console.Write("> ");
                var name = ReadLine(string.Empty, ConsoleColor.Green);

                if (string.IsNullOrEmpty(name))
                {
                    break;
                }

                _console.WriteLine(strings.WHAT_IS_THE_COMPONENT_VERSION);
                Write("[<*>0.1.0<*>]>", ConsoleColor.Green);
                var next = ReadLine("0.1.0", ConsoleColor.Green);

                _console.WriteLine(strings.WHAT_IS_THE_COMPONENT_PRE_RELEASE_TAG);
                Write("[<*>alpha<*>]>", ConsoleColor.Green);
                var tag = ReadLine("alpha", ConsoleColor.Green);

                config.Components.Add(new Component()
                {
                    Name = name,
                    Next = next,
                    Tag = tag,
                    AssemblyInfoFiles = { "AssemblyInfo.cs" },
                });

                _console.WriteLine(strings.ADD_ANOTHER);
            }

            if (!config.Components.Any())
            {
                _console.WriteLine(strings.OK_GOOD_BYE);
                return ReturnCode.Success;
            }

            _console.WriteLine(strings.OK_NOW_ASSIGN_FOLDERS_TO_COMPONENTS);
            _console.WriteLine(strings.PRESS_ENTER_TO_FINISH);
            _console.WriteLine();
            foreach (var directory in _fileSystem.Directory.GetDirectories(_options.Directory))
            {
                var dirName = _fileSystem.Path.GetFileName(directory);
                if (dirName == ".git")
                {
                    continue;
                }

                var componentName = (
                    from c in config.Components
                    where c.Directories.Contains(dirName)
                    select c.Name).FirstOrDefault() ?? string.Empty;

                while (true)
                {
                    if (string.IsNullOrEmpty(componentName))
                    {
                        _console.Write($"{dirName}: ");
                    }
                    else
                    {
                        Write($"{dirName} [<*>{componentName}<*>]> ", ConsoleColor.Green);
                    }

                    var name = ReadLine(componentName, ConsoleColor.Green);

                    if (string.IsNullOrEmpty(name))
                    {
                        break;
                    }

                    if (!config.Components.Any(c => string.Equals(c.Name, name, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        _console.WriteLine(strings.INVALID_COMPONENT);
                        continue;
                    }

                    foreach (var component in config.Components)
                    {
                        if (string.Equals(component.Name, name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            component.Directories.Add(dirName);
                        }
                        else
                        {
                            component.Directories.Remove(dirName);
                        }
                    }
                    break;
                }
            }

            var path = _fileSystem.Path.Combine(dir, "GitComponentVersion.json");

            _fileSystem.File.WriteAllText(path, config.ToString());

            _console.WriteLine();
            _console.WriteLine(strings.YOUR_CHANGES_WERE_WRITTEN_TO, path);

            return ReturnCode.Success;
        }

        private void WriteLine(string format, ConsoleColor color)
        {
            Write(format, color);
            _console.WriteLine();
        }

        private void Write(string format, ConsoleColor color)
        {
            var old = _console.ForegroundColor;

            var start = 0;

            bool colored = false;

            while (true)
            {
                var index = format.IndexOf("<*>", start);
                var sub = index >= 0
                    ? format.Substring(start, index - start)
                    : format.Substring(start);
                

                _console.ForegroundColor = colored ? color : old;

                _console.Write(sub);
                start = index + "<*>".Length;
                colored = !colored;

                if (index < 0)
                {
                    break;
                }
            }
        }

        #endregion

        private string ReadLine(string defaultValue, ConsoleColor color)
        {
            var old = _console.ForegroundColor;
            _console.ForegroundColor = color;

            var input = _console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                input = defaultValue;
            }
            _console.ForegroundColor = old;
            return input;
        }
    }
}
