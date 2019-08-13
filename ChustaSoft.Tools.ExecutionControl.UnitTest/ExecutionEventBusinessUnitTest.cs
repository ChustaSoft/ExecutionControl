using ChustaSoft.Tools.ExecutionControl.Domain;
using ChustaSoft.Tools.ExecutionControl.Entities;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ChustaSoft.Tools.ExecutionControl.UnitTest
{
    [TestClass]
    public class ExecutionEventBusinessUnitTest
    {

        private static IExecutionEventBusiness<Guid> ServiceUnderTest;

        [TestInitialize]
        public void TestInitialize()
        {
            var executionEventRepositoryMocked = new Mock<IExecutionEventRepository<Guid>>();
            executionEventRepositoryMocked.Setup(m => m.Create(It.IsAny<ExecutionEvent<Guid>>())).Returns(true);

            ServiceUnderTest = new ExecutionEventBusiness<Guid>(executionEventRepositoryMocked.Object);
        }


        [TestMethod]
        public void Given_ExecutionIdAndStatusAndMessage_When_Create_Then_TrueRetrived()
        {
            Assert.IsTrue(ServiceUnderTest.Create(Guid.NewGuid(), ExecutionStatus.Aborted, ""));
        }

    }
}
