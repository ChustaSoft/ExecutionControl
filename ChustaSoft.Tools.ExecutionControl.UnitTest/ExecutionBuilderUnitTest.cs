using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;


namespace ChustaSoft.Tools.ExecutionControl.UnitTest
{
    [TestClass]
    public class ExecutionBuilderUnitTest
    {

        [TestMethod]
        public void Given_Builder_When_CreateDefinition_Then_DefinitionReturned()
        {
            string testName = "TestName", testDescription = "TestDescription";
            var builder = new ExecutionBuilder<Guid>();

            builder.Generate(c =>
            {
                c.New(testName, testDescription);
            });

            var result = builder.Build();
            var elementCreated = result.First();

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(elementCreated.Name, testName);
            Assert.AreEqual(elementCreated.Description, testDescription);
        }

        [TestMethod]
        public void Given_CreatedDefinition_When_SetType_Then_DefinitionWithTypeReturned()
        {
            string testName = "TestName", testDescription = "TestDescription";
            var type = ExecutionType.Automatic;
            var builder = new ExecutionBuilder<Guid>();

            builder.Generate(c =>
            {
                c.New(testName, testDescription).SetType(type);
            });

            var result = builder.Build();
            var elementCreated = result.First();

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(elementCreated.Name, testName);
            Assert.AreEqual(elementCreated.Description, testDescription);
            Assert.AreEqual(elementCreated.Type, type);
        }

        [TestMethod]
        public void Given_CreatedDefinition_When_AddModule_Then_DefinitionWithModuleReturned()
        {
            string testName = "TestName", testDescription = "TestDescription";
            string testNameModule = "ModuleName", testDescriptionModule = "ModuleDescription";
            var type = ExecutionType.Automatic;
            var builder = new ExecutionBuilder<Guid>();

            builder.Generate(c =>
            {
                c.New(testName, testDescription).SetType(type).AddModule(testNameModule, testDescriptionModule);
            });

            var result = builder.Build();
            var elementCreated = result.First();

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(elementCreated.Name, testName);
            Assert.AreEqual(elementCreated.Description, testDescription);
            Assert.AreEqual(elementCreated.Type, type);
            Assert.AreEqual(elementCreated.ModuleDefinitions.Count(), 1);
        }

    }
}
