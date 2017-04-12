using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GitComponentVersion.Commands;
using GitComponentVersion.Configuration;

namespace GitComponentVersion.Busi
{
    /// <summary>
    /// A class that can load a <see cref="GitComponentVersionFile"/> from disk.
    /// </summary>
    public interface IGitComponentVersionFileFactory
    {
        /// <summary>
        /// Finds a GitDepend.json file in the given directory and loads it into memory.
        /// </summary>
        /// <param name="directory">The directory to start in.</param>
        /// <param name="dir">The directory where GitComponentVersionFile.json was found.</param>
        /// <param name="code">The return code indicating if the load was successful, or which error occurred.</param>
        /// <returns>A <see cref="GitComponentVersionFile"/> or null if none could be loaded.</returns>
        GitComponentVersionFile LoadFromDirectory(string directory, out string dir, out ReturnCode code);
    }
}
