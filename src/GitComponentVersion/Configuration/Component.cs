using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitComponentVersion.Configuration
{
    /// <summary>
    /// Represents an individual Component
    /// </summary>
    public class Component
    {
        private HashSet<string> _directories;
        private HashSet<string> _assemblyInfoFiles;

        /// <summary>
        /// The name of the component
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The next version
        /// </summary>
        [JsonProperty("next")]
        public string Next { get; set; }

        /// <summary>
        /// The pre-release tag
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// The name directories that are a member of this component.
        /// </summary>
        [JsonProperty("dirs")]
        public HashSet<string> Directories => _directories ?? (_directories = new HashSet<string>());

        /// <summary>
        /// The name of AssemblyInfo files to modify during an update.
        /// </summary>
        [JsonProperty("assemblyinfo")]
        public HashSet<string> AssemblyInfoFiles => _assemblyInfoFiles ?? (_assemblyInfoFiles = new HashSet<string>());
    }
}