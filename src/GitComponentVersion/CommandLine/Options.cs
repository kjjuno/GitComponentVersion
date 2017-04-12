using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using GitComponentVersion.Commands;

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
    }
}
