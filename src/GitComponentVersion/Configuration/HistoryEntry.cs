using Newtonsoft.Json;

namespace GitComponentVersion.Configuration
{
    /// <summary>
    /// An entry in the release history section of a <see cref="Component"/>
    /// </summary>
    public class HistoryEntry
    {
        /// <summary>
        /// The version.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// The git sha
        /// </summary>
        [JsonProperty("sha")]
        public string Sha { get; set; }

        /// <summary>
        /// The notes.
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}