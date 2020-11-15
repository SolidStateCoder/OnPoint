using System;

namespace OnPoint.Universal
{
    // Models the response from a service or data call.
    public class ExecutionResultMessage
    {
        public string Message { get; protected set; }
        public ExecutionResult ExecutionResult { get; protected set; }

        public static ExecutionResultMessage Success { get; } = new ExecutionResultMessage(ExecutionResult.Success);
        public static ExecutionResultMessage NoOp { get; } = new ExecutionResultMessage(ExecutionResult.NoOp);

        public bool IsSuccess { get => ExecutionResult == ExecutionResult.Success; }

        public ExecutionResultMessage(ExecutionResult executionResult, string message = default)
        {
            ExecutionResult = ExecutionResult.NoOp;
            Message = message;
            Update(executionResult, message);
        }

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

        public ExecutionResultMessage Update(ExecutionResult executionResult, string message = default)
        {
            ExecutionResult = executionResult;
            Message = message;
            if (Message == null)
            {
                switch (ExecutionResult)
                {
                    case ExecutionResult.Failure:
                        Message = "A failure occurred.";
                        break;
                    case ExecutionResult.NoOp:
                        Message = "No action was taken.";
                        break;
                    case ExecutionResult.NoResult:
                        Message = "No result was provided.";
                        break;
                    case ExecutionResult.Success:
                        Message = "Everything worked properly.";
                        break;
                }
            }
            return this;
        }
    }


    public class ExecutionResultMessage<T> : ExecutionResultMessage
    {
        public T Payload { get; private set; }

        public ExecutionResultMessage(ExecutionResult executionResult, T payload = default, string message = default) : base(executionResult, message)
        {
            Payload = payload;
        }

        public ExecutionResultMessage Update(ExecutionResult executionResult, T payload = default, string message = default)
        {
            Payload = payload;
            return Update(executionResult, message);
        }
    }
}