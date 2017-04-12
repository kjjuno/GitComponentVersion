using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using GitComponentVersion.Commands;
using Newtonsoft.Json;

namespace GitComponentVersion.CommandLine
{
    class Options
    {
        private static Options _default;
        public static Options Default => _default ?? (_default = new Options());

        private Options()
        {
        }

        [VerbOption(InitCommand.Name, HelpText = "Assists you in creating a GitComponentVersion.json file")]
        public InitSubOptions InitVerb { get; set; } = new InitSubOptions();

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            //var versionUpdateChecker = DependencyInjection.Resolve<IVersionUpdateChecker>();

            //var appendString = versionUpdateChecker.CheckVersion("gitdepend", "kjjuno");

            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));// + appendString;
        }

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length == 3 && string.Equals(args[1], "help", StringComparison.OrdinalIgnoreCase))
            {
                verb = args[2];
            }

            return HelpText.AutoBuild(this, verb);
        }

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #endregion
    }
}
