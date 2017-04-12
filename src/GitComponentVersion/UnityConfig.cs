using System.IO.Abstractions;
using GitComponentVersion.Busi;
using Microsoft.Practices.Unity;

namespace GitComponentVersion
{
    /// <summary>
    /// Configures Unity for the project.
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Registers object mappings on the given <see cref="UnityContainer"/>
        /// </summary>
        /// <param name="container">The <see cref="UnityContainer"/> to use for object mappings.</param>
        public static void RegisterTypes(UnityContainer container)
        {
            // Busi
            container
                .RegisterType<IConsole, ConsoleWrapper>()
                .RegisterType<IGitComponentVersionFileFactory, GitComponentVersionFileFactory>()
                .RegisterType<IUiStrings, UiStrings>();

            // External
            container
                .RegisterType<IFileSystem, FileSystem>();
        }
    }
}