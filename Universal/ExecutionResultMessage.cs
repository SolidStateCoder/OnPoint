using System;

namespace OnPoint.Universal
{
    /// <summary>
    /// Models the response from a service or data call.
    /// </summary>
    public class ExecutionResultMessage
    {
        /// <summary>
        /// The text of the result.
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// The result of the action.
        /// </summary>
        public ExecutionResult ExecutionResult { get; protected set; }

        public static ExecutionResultMessage Success { get; } = new ExecutionResultMessage(ExecutionResult.Success);
        public static ExecutionResultMessage NoOp { get; } = new ExecutionResultMessage(ExecutionResult.NoOp);

        public bool IsSuccess { get => ExecutionResult == ExecutionResult.Success; }

        /// <summary>
        /// Creates a new ExecutionResultMessage.
        /// </summary>
        /// <param name="executionResult">The result of the action</param>
        /// <param name="message">The text to display</param>
        public ExecutionResultMessage(ExecutionResult executionResult, string message = default)
        {
            ExecutionResult = ExecutionResult.NoOp;
            Message = message;
            Update(executionResult, message);
        }

        /// <summary>
        /// Adds <paramref name="message"/> to the <see cref="Message" />; fluent.
        /// </summary>
        /// <param name="message">The text to add</param>
        /// <returns>This ExecutionResultMessage</returns>
        public ExecutionResultMessage Append(string message)
        {
            if (message != null)
            {
                if (Message.IsNothing())
                {
                    Message = message;
                }
                else
                {
                    Message += Environment.NewLine + message;
                }
            }
            return this;
        }

        /// <summary>
        /// Change the <see cref="ExecutionResult"/>, and optionally <see cref="Message"/>.
        /// </summary>
        /// <param name="executionResult">The new <see cref="ExecutionResult"/></param>
        /// <param name="message">The new <see cref="Message"/></param>
        /// <returns></returns>
        public ExecutionResultMessage Update(ExecutionResult executionResult, string message = default)
        {
            ExecutionResult = executionResult;
            Message = message ?? ExecutionResult switch
            {
                ExecutionResult.Failure => "A failure occurred.",
                ExecutionResult.NoOp => "No action was taken.",
                ExecutionResult.NoResult => "No result was provided.",
                ExecutionResult.Success => "Everything worked properly.",
                _ => throw new NotImplementedException()
            };
            return this;
        }
    }


    /// <summary>
    /// Adds a <see cref="Payload"/> to ExecutionResultMessage.
    /// </summary>
    /// <typeparam name="T">The type of item to reference</typeparam>
    public class ExecutionResultMessage<T> : ExecutionResultMessage
    {
        /// <summary>
        /// The item to reference
        /// </summary>
        public T Payload { get; private set; }

        /// <summary>
        /// Create a new ExecutionResultMessage.
        /// </summary>
        /// <param name="executionResult">The result of the action</param>
        /// <param name="payload">The item to reference</param>
        /// <param name="message">The text to display</param>
        public ExecutionResultMessage(ExecutionResult executionResult, T payload = default, string message = default) : base(executionResult, message)
        {
            Payload = payload;
        }

        /// <summary>
        /// Update the <see cref="ExecutionResultMessage.ExecutionResult"/> and optionally the <see cref="Payload"/> and <see cref="ExecutionResultMessage.Message"/>.
        /// </summary>
        /// <param name="executionResult"></param>
        /// <param name="payload"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ExecutionResultMessage Update(ExecutionResult executionResult, T payload = default, string message = default)
        {
            Payload = payload;
            return Update(executionResult, message);
        }
    }
}