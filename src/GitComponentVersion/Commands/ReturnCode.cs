namespace GitComponentVersion.Commands
{
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
        /// Indicates that the specified directory does not exist.
        /// </summary>
        [ResxKey("RET_DIRECTORY_DOES_NOT_EXIST")]
        DirectoryDoesNotExist = 2,

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
}