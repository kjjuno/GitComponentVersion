using System;
using System.Collections.Generic;
using System.IO;
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

            AddSuggestedDirectories(config, dir);

            _console.WriteLine(strings.DONE_WITH_SUGGESTED_DIRECTORIES);
            _console.WriteLine();
            _console.WriteLine(strings.YOU_CAN_ADD_MORE);

            AddAdditionalComponents(config, dir);

            if (!config.Components.Any())
            {
                _console.WriteLine(strings.OK_GOOD_BYE);
                return ReturnCode.Success;
            }

            AssignProjectFoldersToComponents(config, dir);

            var path = _fileSystem.Path.Combine(dir, "GitComponentVersion.json");

            _fileSystem.File.WriteAllText(path, config.ToString());

            _console.WriteLine();
            _console.WriteLine(strings.YOUR_CHANGES_WERE_WRITTEN_TO, path);

            return ReturnCode.Success;
        }

        private void AssignProjectFoldersToComponents(GitComponentVersionFile config, string dir)
        {
            _console.WriteLine(strings.OK_NOW_ASSIGN_FOLDERS_TO_COMPONENTS);
            _console.WriteLine(strings.PRESS_ENTER_TO_FINISH);
            _console.WriteLine();

            var uri = new Uri(dir + "/");

            var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(dir);
            var query =
                from d in dirInfo.GetDirectories("*", SearchOption.AllDirectories)
                let path = new Uri(d.Parent.FullName + "/") 
                let relPath = uri.MakeRelativeUri(path).ToString().TrimEnd('/')
                where d.GetFiles("*.csproj").Any() &&
                      !config.Components.Any(c => c.Directories.Contains(relPath))
                select d;

            foreach (var directory in query)
            {
                if (directory.Name == ".git")
                {
                    continue;
                }

                var componentName = (
                    from c in config.Components
                    where c.Directories.Contains(directory.Name)
                    select c.Name).FirstOrDefault() ?? string.Empty;

                while (true)
                {
                    if (string.IsNullOrEmpty(componentName))
                    {
                        _console.Write($"{directory.Name}: ");
                    }
                    else
                    {
                        Write($"{directory.Name} [<*>{componentName}<*>]> ", ConsoleColor.Green);
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
                            component.Directories.Add(directory.Name);
                        }
                        else
                        {
                            component.Directories.Remove(directory.Name);
                        }
                    }
                    break;
                }
            }
        }

        private void AddAdditionalComponents(GitComponentVersionFile config, string dir)
        {
            while (true)
            {
                _console.WriteLine(strings.PRESS_ENTER_TO_FINISH);
                _console.WriteLine();

                _console.WriteLine(strings.ENTER_COMPONENT_NAME);
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
        }

        private void AddSuggestedDirectories(GitComponentVersionFile config, string dir)
        {
            var suggested = SuggestComponentDirectories(config, dir);

            foreach (var suggestedDir in suggested)
            {
                _console.WriteLine("Is this a component?");
                Write($"{suggestedDir} [<*>Y/n<*>]> ", ConsoleColor.Green);
                var answer = ReadLine("Y", ConsoleColor.Green);

                if (string.Equals(answer, "y", StringComparison.CurrentCultureIgnoreCase))
                {
                    var dirName = _fileSystem.Path.GetFileName(suggestedDir);
                    _console.WriteLine(strings.ENTER_COMPONENT_NAME);
                    Write($"[<*>{dirName}<*>]> ", ConsoleColor.Green);
                    var name = ReadLine(dirName, ConsoleColor.Green);

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
                        Directories = { suggestedDir },
                        AssemblyInfoFiles = { "AssemblyInfo.cs" }
                    });

                    WriteLine($"<*>Adding the '{suggestedDir}' component<*>", ConsoleColor.Green);
                }

            }
        }

        private List<string> SuggestComponentDirectories(GitComponentVersionFile config, string directory)
        {
            var uri = new Uri(directory + "/");
            var info = _fileSystem.DirectoryInfo.FromDirectoryName(directory);

            var query =
                from d in info.GetDirectories("*", SearchOption.AllDirectories)
                where d.GetFiles("*.csproj").Any()
                let path = new Uri(d.Parent.FullName + "/")
                select uri.MakeRelativeUri(path).ToString().TrimEnd('/');

            return query
                .Distinct()
                .Where(d => !string.IsNullOrEmpty(d) && !config.Components.Any(c => c.Directories.Contains(d)))
                .ToList();
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
