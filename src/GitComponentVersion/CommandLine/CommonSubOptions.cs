using CommandLine;

namespace GitComponentVersion.CommandLine
{
    /// <summary>
    /// Provids command line options that are common to many verbs.
    /// </summary>
    public abstract class CommonSubOptions
    {
        /// <summary>
        /// The directory that should be processed.
        /// </summary>
        [Option("dir", Required = false, HelpText = "The directory to process. The current working directory will be used if this option is ignored.")]
        public string Directory { get; set; }
    }
}