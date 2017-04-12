using System.Reflection;
using System.Resources;
using GitComponentVersion.Resources;

namespace GitComponentVersion.Busi
{
    /// <summary>
    /// An implementation of <see cref="IUiStrings"/> that pulls strings from the resource files.
    /// </summary>
    public class UiStrings : IUiStrings
    {
        #region Implementation of IUiStrings

        /// <summary>
        /// Gets the string associated with the given key.
        /// </summary>
        /// <param name="key">The resource string key.</param>
        /// <returns>The string from the resource files.</returns>
        public string GetString(string key)
        {
            return strings.ResourceManager.GetString(key);
        }

        #endregion
    }
}