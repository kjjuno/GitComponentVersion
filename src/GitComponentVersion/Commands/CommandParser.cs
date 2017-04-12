using System;
using System.Collections.Generic;
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

    /// <summary>
    /// An action that will be taken as the result of parsing the command line options.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>The return code.</returns>
        ReturnCode Execute();
    }

    /// <summary>
    /// Project Return Codes.
    /// </summary>
    public enum ReturnCode
    {
        /// <summary>
        /// Indicates the program ran successfully.
        /// </summary>
        [ResxKey("RET_SUCCESS")]
        Success = 0,

        /// <summary>
        /// Indicates that the target directory is not a git repository.
        /// </summary>
        [ResxKey("RET_GIT_REPO_NOT_FOUND")]
        GitRepositoryNotFound = 1,

        /// <summary>
        /// Indicates that there was a failure while running a git command.
        /// </summary>
        [ResxKey("RET_GIT_COMMAND_FAILED")]
        FailedToRunGitCommand = 2,

        /// <summary>
        /// Indicates that there was a failure while running a nuget command.
        /// </summary>
        [ResxKey("RET_NUGET_COMMAND_FAILED")]
        FailedToRunNugetCommand = 3,

        /// <summary>
        /// Indicates that a dependency failed to run the build script successfully.
        /// </summary>
        [ResxKey("RET_BUILD_SCRIPT_FAILED")]
        FailedToRunBuildScript = 4,

        /// <summary>
        /// Indicates that the specified directory does not exist.
        /// </summary>
        [ResxKey("RET_DIRECTORY_DOES_NOT_EXIST")]
        DirectoryDoesNotExist = 5,

        /// <summary>
        /// Indicates that there was a failre when trying to create the nuget cache directory.
        /// </summary>
        [ResxKey("RET_CREATE_CACHE_DIR_FAILED")]
        CouldNotCreateCacheDirectory = 6,

        /// <summary>
        /// Indicates that the specified dependency url is invalid. The only supported format is https
        /// </summary>
        [ResxKey("RET_INVALID_URI_FORMAT")]
        InvalidUrlFormat = 7,

        /// <summary>
        /// Specifies that a dependency is missing in the GitDepend.json
        /// </summary>
        [ResxKey("RET_MISSING_DEPENDENCY")]
        MissingDependency = 8,

        /// <summary>
        /// Specifies that an invalid branch is checked out on a dependency.
        /// </summary>
        [ResxKey("RET_INVALID_BRANCH_CHECKED_OUT")]
        InvalidBranchCheckedOut = 9,

        /// <summary>
        /// Specifies the dependency are packages not built 
        /// </summary>
        [ResxKey("RET_DEPENDENCY_PACKAGES_NOT_BUILT")]
        DependencyPackagesNotBuilt = 10,

        /// <summary>
        /// Specifies the dependency packages are mismatching
        /// </summary>
        [ResxKey("RET_DEPENDENCY_PACKAGES_MISTMATCH")]
        DependencyPackagesMisMatch = 11,

        /// <summary>
        /// The name did not match the requested dependency
        /// </summary>
        [ResxKey("RET_NAME_DID_NOT_MATCH")]
        NameDidNotMatchRequestedDependency = 12,

        /// <summary>
        /// The dependency already exists
        /// </summary>
        [ResxKey("RET_DEPENDENCY_ALREADY_EXISTS")]
        DependencyAlreadyExists = 13,

        /// <summary>
        /// Specifies that the artifacts directory could not be found
        /// </summary>
        [ResxKey("RET_ARTIFACTS_DIR_NOT_FOUND")]
        FailedToLocateArtifactsDir = 14,

        /// <summary>
        /// Specifies that there is no configuration found for the current folder
        /// </summary>
        [ResxKey("RET_DEPEND_FILE_NOT_FOUND")]
        ConfigurationFileDoesNotExist = 15,

        /// <summary>
        /// Indicates build script in config file could not be found
        /// </summary>
        [ResxKey("RET_BUILD_SCRIPT_NOT_FOUND")]
        BuildScriptNotFound = 16,

        /// <summary>
        /// Indicates the supplied arguments were invalid.
        /// </summary>
        [ResxKey("RET_INVALID_ARGS")]
        InvalidArguments = 9997,

        /// <summary>
        /// Indicates that the command parser did not understand the specified command.
        /// </summary>
        [ResxKey("RET_INVALID_COMMAND")]
        InvalidCommand = 9998,

        /// <summary>
        /// Indicates that something went wrong, but the cause could not be determined.
        /// </summary>
        [ResxKey("RET_UNKNOWN_ERROR")]
        UnknownError = 9999,
    }

    /// <summary>
    /// Provides a key for strings in the resx files.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ResxKeyAttribute : Attribute
    {
        /// <summary>
        /// The resource key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Creates a new <see cref="ResxKeyAttribute"/> witht he specified key.
        /// </summary>
        /// <param name="key">The key for the resx files.</param>
        public ResxKeyAttribute(string key)
        {
            Key = key;
        }
    }
}
