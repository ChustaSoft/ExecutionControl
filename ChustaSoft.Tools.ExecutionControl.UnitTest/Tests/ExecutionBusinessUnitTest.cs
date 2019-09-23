using ChustaSoft.Tools.ExecutionControl.Configuration;
using ChustaSoft.Tools.ExecutionControl.Domain;
using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ChustaSoft.Tools.ExecutionControl.UnitTest.Tests
{
    [TestClass]
    public class ExecutionBusinessUnitTest
    {

        private const int ABORT_MINUTES = 5;

        private static IExecutionBusiness<Guid> ServiceUnderTest;


        [ClassInitialize]
        public static void TestInitialize(TestContext context)
        {
            var mockedConfiguration = new ExecutionControlConfiguration { MinutesToAbort = ABORT_MINUTES };

            var mockedProcessDefinitionRepository = new Mock<IProcessDefinitionRepository<Guid>>();
            mockedProcessDefinitionRepository.Setup(m => m.Get(MockedProcessDefinition.Name)).Returns(MockedProcessDefinition);
            mockedProcessDefinitionRepository.Setup(m => m.Get(string.Empty)).Throws<InvalidOperationException>();

            var mockedExecutionRepository = new Mock<IExecutionRepository<Guid>>();
            mockedExecutionRepository.Setup(m => m.Save(It.IsAny<Execution<Guid>>())).Returns(true).Callback<Execution<Guid>>(e => e.Id = Guid.NewGuid());
            mockedExecutionRepository.Setup(m => m.Update(It.IsAny<Execution<Guid>>())).Returns(true).Callback<Execution<Guid>>(e => e.Id = Guid.NewGuid());
            mockedExecutionRepository.Setup(m => m.GetLastDead(It.IsAny<Execution<Guid>>())).Returns(MockedDeadExecution);
            mockedExecutionRepository.Setup(m => m.GetLastCompleted(It.Is<Execution<Guid>>(x => x.Id == MockedExecutionPreviousFinished.Id))).Returns(MockedFinishedEndExecution);
            mockedExecutionRepository.Setup(m => m.GetLastCompleted(It.Is<Execution<Guid>>(x => x.Id == MockedExecutionPreviousAborted.Id))).Returns(MockedAbortedEndExecution);
            mockedExecutionRepository.Setup(m => m.GetLastCompleted(It.Is<Execution<Guid>>(x => x.Id == MockedExecutionPreviousNull.Id))).Returns(MockedNullPreviousExecution);
            mockedExecutionRepository.Setup(m => m.GetLastCompleted(It.Is<Execution<Guid>>(x => x.Id == MockedExecutionPreviousAbortOverdue.Id))).Returns(MockedRunningOverdueExecution);

            ServiceUnderTest = new ExecutionBusiness<Guid>(mockedConfiguration, mockedProcessDefinitionRepository.Object, mockedExecutionRepository.Object);
        }


        [TestMethod]
        public void Given_ExistingProcessName_When_Register_Then_ExecutionCreated()
        {
            var testName = MockedProcessDefinition.Name;

            var result = ServiceUnderTest.Register(testName);

            Assert.IsTrue(result.BeginDate <= DateTime.UtcNow && result.BeginDate > DateTime.MinValue);
            Assert.IsTrue(result.Id != Guid.Empty);
            Assert.AreEqual(result.Host, Environment.MachineName);
            Assert.AreEqual(result.Result, ExecutionResult.Unknown);
            Assert.AreEqual(result.Status, ExecutionStatus.Waiting);
            Assert.AreEqual(result.ProcessDefinitionId, MockedProcessDefinition.Id);
        }

        [TestMethod]
        public void Given_UnexistingProcessName_When_Register_Then_ExceptionThrown()
        {
            Assert.ThrowsException<InvalidOperationException>(() => ServiceUnderTest.Register(string.Empty));
        }

        [TestMethod]
        public void Given_ExistingExecution_When_Start_Then_StatusChanged()
        {
            var testExecution = MockedExecution;

            ServiceUnderTest.Start(testExecution);

            Assert.AreEqual(testExecution.Status, ExecutionStatus.Running);
        }

        [TestMethod]
        public void Given_ExistingExecution_When_Abort_Then_PreviousExecutionAborted()
        {
            var testExecution = MockedExecution;

            ServiceUnderTest.Abort(testExecution);

            Assert.AreEqual(testExecution.Status, ExecutionStatus.Waiting);
        }

        [TestMethod]
        public void Given_ExistingExecution_When_Block_Then_ExecutionBlocked()
        {
            var testExecution = MockedExecution;

            ServiceUnderTest.Block(testExecution);

            Assert.AreEqual(testExecution.Status, ExecutionStatus.Blocked);
        }

        [TestMethod]
        public void Given_ExistingExecutionAndResult_When_Complete_Then_ExecutionFinishedWithStatus()
        {
            var testExecution = MockedExecution;
            var testResult = ExecutionResult.Success;

            ServiceUnderTest.Complete(testExecution, testResult);

            Assert.AreEqual(testExecution.Result, testResult);
            Assert.AreEqual(testExecution.Status, ExecutionStatus.Finished);
        }

        [TestMethod]
        public void Given_ExistingExecutionWithPreviousFinished_When_IsAllowed_Then_AllowedRetrived()
        {
            var testExecution = MockedExecutionPreviousFinished;

            var result = ServiceUnderTest.IsAllowed(testExecution);

            Assert.AreEqual(ExecutionAvailability.Available, result);
        }

        [TestMethod]
        public void Given_ExistingExecutionWithPreviousAborted_When_IsAllowed_Then_AllowedRetrived()
        {
            var testExecution = MockedExecutionPreviousAborted;

            var result = ServiceUnderTest.IsAllowed(testExecution);

            Assert.AreEqual(ExecutionAvailability.Available, result);
        }

        [TestMethod]
        public void Given_ExistingExecutionWithoutAnyPrevious_When_IsAllowed_Then_AllowedRetrived()
        {
            var testExecution = MockedExecutionPreviousNull;

            var result = ServiceUnderTest.IsAllowed(testExecution);

            Assert.AreEqual(ExecutionAvailability.Available, result);
        }

        [TestMethod]
        public void Given_ExistingExecutionWithPreviousAbortOverdue_When_IsAllowed_Then_AbortRetrived()
        {
            var testExecution = MockedExecutionPreviousAbortOverdue;

            var result = ServiceUnderTest.IsAllowed(testExecution);

            Assert.AreEqual(ExecutionAvailability.Abort, result);
        }

        internal static ProcessDefinition<Guid> MockedProcessDefinition
            => new ProcessDefinition<Guid>
            {
                Id = Guid.Parse("05e42964-b138-41c8-99d8-fca4aec4861e"),
                Active = true,
                Name = "TestProcess",
                Description = "Test Description"
            };

        internal static Execution<Guid> MockedDeadExecution
            => new Execution<Guid>
            {
                Id = Guid.Parse("05e42964-b138-41c8-99d8-fca4aec4861e"),
                BeginDate = DateTime.UtcNow.AddMinutes(-1)
            };

        internal static Execution<Guid> MockedFinishedEndExecution
            => new Execution<Guid>
            {
                Id = Guid.Parse("05e42964-b138-41c8-99d8-fca4aec4861e"),
                BeginDate = DateTime.UtcNow.AddMinutes(-1),
                Status = ExecutionStatus.Finished
            };

        internal static Execution<Guid> MockedAbortedEndExecution
            => new Execution<Guid>
            {
                Id = Guid.Parse("05e42964-b138-41c8-99d8-fca4aec4861e"),
                BeginDate = DateTime.UtcNow.AddMinutes(-1),
                Status = ExecutionStatus.Aborted
            };

        internal static Execution<Guid> MockedNullPreviousExecution
            => null;

        internal static Execution<Guid> MockedRunningOverdueExecution
            => new Execution<Guid>
            {
                Id = Guid.Parse("05e42964-b138-41c8-99d8-fca4aec4861e"),
                BeginDate = DateTime.UtcNow.AddMinutes(-10),
                Status = ExecutionStatus.Running
            };

        internal static Execution<Guid> MockedExecution
            => new Execution<Guid>
            {
                Id = Guid.Parse("05e42964-b138-41c8-99d8-fca4aec4861e"),
                BeginDate = DateTime.UtcNow.AddMinutes(-1)
            };

        internal static Execution<Guid> MockedExecutionPreviousFinished
            => new Execution<Guid>
            {
                Id = Guid.Parse("05e42964-b138-41c8-99d8-fca4aec4861f")
            };

        internal static Execution<Guid> MockedExecutionPreviousAborted
            => new Execution<Guid>
            {
                Id = Guid.Parse("05e42964-b198-41c8-99d8-fca4aec4860a")
            };

        internal static Execution<Guid> MockedExecutionPreviousNull
            => new Execution<Guid>
            {
                Id = Guid.Parse("05e42964-b138-41c8-99d8-fca4aec4860a")
            };

        internal static Execution<Guid> MockedExecutionPreviousAbortOverdue
            => new Execution<Guid>
            {
                Id = Guid.Parse("05e42964-a138-41c8-99d8-fca4aec4860b")
            };

    }
}
