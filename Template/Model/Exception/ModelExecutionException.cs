namespace CYCLONE.Template.Model.Exception
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// An exception indicating that an error occurred when executing a simulation model.
    /// </summary>
    [Serializable]
    public class ModelExecutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelExecutionException"/> class.
        /// </summary>
        public ModelExecutionException()
        {
            // Do nothing...
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelExecutionException"/> class with a
        /// reference to the inner exception that is the cause of this exception, and context.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        /// <param name="context">
        /// The context of the original exception.
        /// </param>
        public ModelExecutionException(Exception innerException, object context)
            : base(null, innerException)
        {
            Context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelExecutionException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ModelExecutionException(string message)
            : base(message)
        {
            // Do nothing...
        }

        /// <summary>
        /// Gets the context of the original exception.
        /// </summary>
        /// <value>
        /// The context of the original exception.
        /// </value>
        public object? Context { get; private set; }
    }
}
