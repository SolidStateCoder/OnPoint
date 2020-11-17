using NUnit.Framework;
using OnPoint.Universal;
using System;

namespace OnPoint.UnitTests
{
    public class ExecutionResultMessageTests
    {
        ExecutionResultMessage _Foo = new ExecutionResultMessage(ExecutionResult.Failure, "Foo");
        ExecutionResultMessage<int> _Five = new ExecutionResultMessage<int>(ExecutionResult.Failure, 5, "Five");

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AnyNonNulls()
        {
            Assert.AreEqual(ExecutionResultMessage.NoOp.ExecutionResult, ExecutionResult.NoOp);
            Assert.AreEqual(ExecutionResultMessage.Success.ExecutionResult, ExecutionResult.Success);
            _Foo.Append("Bar");
            Assert.AreEqual(_Foo.Message, $"Foo{Environment.NewLine}Bar");
            _Foo.Update(ExecutionResult.Success, "Foo");
            Assert.AreEqual(_Foo.Message, "Foo");
            Assert.AreEqual(_Foo.ExecutionResult, ExecutionResult.Success);
            Assert.AreEqual(_Five.Payload, 5);
        }
    }
}