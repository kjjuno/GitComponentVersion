using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GitComponentVersion.Configuration
{
    /// <summary>
    /// Represents GitComponentVersionFile.json in memory.
    /// </summary>
    public class GitComponentVersionFile
    {
        private List<Component> _components; 

        /// <summary>
        /// The components section
        /// </summary>
        [JsonProperty("components")]
        public List<Component> Components => _components ?? (_components = new List<Component>());

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
