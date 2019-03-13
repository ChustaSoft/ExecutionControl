using ChustaSoft.Common.Exceptions;
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

        [TestMethod]
        public void Given_CreatedDefinition_When_AddModuleWithConcurrentBoolean_Then_DefinitionWithModuleReturned()
        {
            string testName = "TestName", testDescription = "TestDescription";
            string testNameModule = "ModuleName", testDescriptionModule = "ModuleDescription";
            var type = ExecutionType.Automatic;
            var builder = new ExecutionBuilder<Guid>();

            builder.Generate(c =>
            {
                c.New(testName, testDescription).SetType(type).AddModule(testNameModule, testDescriptionModule, true);
            });

            var result = builder.Build();
            var elementCreated = result.First();

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(elementCreated.Name, testName);
            Assert.AreEqual(elementCreated.Description, testDescription);
            Assert.AreEqual(elementCreated.Type, type);
            Assert.AreEqual(elementCreated.ModuleDefinitions.Count(), 1);
            Assert.IsTrue(elementCreated.ModuleDefinitions.Any(x => x.Concurrent));
        }

        [TestMethod]
        public void Given_CreatedDefinition_When_AddModuleMultipleTimes_Then_DefinitionWithModulesReturned()
        {
            string testName = "TestName", testDescription = "TestDescription";
            string testNameModule = "ModuleName", testDescriptionModule = "ModuleDescription";
            var type = ExecutionType.Automatic;
            var builder = new ExecutionBuilder<Guid>();

            builder.Generate(c =>
            {
                c.New(testName, testDescription).SetType(type)
                    .AddModule(testNameModule, testDescriptionModule)
                    .AddModule(testNameModule, testDescriptionModule);
            });

            var result = builder.Build();
            var elementCreated = result.First();

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(elementCreated.Name, testName);
            Assert.AreEqual(elementCreated.Description, testDescription);
            Assert.AreEqual(elementCreated.Type, type);
            Assert.AreEqual(elementCreated.ModuleDefinitions.Count(), 2);
        }

        [TestMethod]
        public void Given_CreatedDefinition_When_AddModuleMultipleTimesWithDifferentConcurrentBooleans_Then_DefinitionWithModulesReturned()
        {
            string testName = "TestName", testDescription = "TestDescription";
            string testNameModule = "ModuleName", testDescriptionModule = "ModuleDescription";
            var type = ExecutionType.Automatic;
            var builder = new ExecutionBuilder<Guid>();

            builder.Generate(c =>
            {
                c.New(testName, testDescription).SetType(type)
                    .AddModule(testNameModule, testDescriptionModule)
                    .AddModule(testNameModule, testDescriptionModule, true);
            });

            var result = builder.Build();
            var elementCreated = result.First();

            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(elementCreated.Name, testName);
            Assert.AreEqual(elementCreated.Description, testDescription);
            Assert.AreEqual(elementCreated.Type, type);
            Assert.AreEqual(elementCreated.ModuleDefinitions.Count(), 2);
            Assert.IsTrue(elementCreated.ModuleDefinitions.Any(x => x.Concurrent));
            Assert.IsTrue(elementCreated.ModuleDefinitions.Any(x => !x.Concurrent));
        }

        [TestMethod]
        public void Given_CreatedDefinitions_When_FullTest_Then_DefinitionsReturned()
        {
            string testName = "TestName", testDescription = "TestDescription";
            string testNameModule = "ModuleName", testDescriptionModule = "ModuleDescription";
            var type = ExecutionType.Automatic;
            var builder = new ExecutionBuilder<Guid>();

            builder.Generate(c =>
            {
                c.New(testName + 1, testDescription + 1).SetType(type)
                    .AddModule(testNameModule + 1, testDescriptionModule + 1)
                    .AddModule(testNameModule + 1, testDescriptionModule + 1);

                c.New(testName + 2, testDescription + 2).SetType(type)
                    .AddModule(testNameModule + 2, testDescriptionModule + 2)
                    .AddModule(testNameModule + 2, testDescriptionModule + 2);
            });

            var result = builder.Build();
            var firstElementCreated = result.ToArray()[0];
            var secondElementCreated = result.ToArray()[1];

            Assert.AreEqual(result.Count(), 2);

            Assert.AreEqual(firstElementCreated.Name, testName + 1);
            Assert.AreEqual(firstElementCreated.Description, testDescription + 1);
            Assert.AreEqual(firstElementCreated.Type, type);
            Assert.AreEqual(firstElementCreated.ModuleDefinitions.Count(), 2);
            
            Assert.AreEqual(secondElementCreated.Name, testName + 2);
            Assert.AreEqual(secondElementCreated.Description, testDescription + 2);
            Assert.AreEqual(secondElementCreated.Type, type);
            Assert.AreEqual(secondElementCreated.ModuleDefinitions.Count(), 2);
        }

        [TestMethod]
        public void Given_CreatedDefinitions_When_FullTestEnumTyped_Then_DefinitionsEnumTypedReturned()
        {
            string testDescription = "TestDescription", testNameModule = "ModuleName", testDescriptionModule = "ModuleDescription";
            var type = ExecutionType.Automatic;
            var builder = new ExecutionBuilder<TestEnum, Guid>();

            builder.Generate(c =>
            {
                c.New(TestEnum.Test1, testDescription + 1).SetType(type)
                    .AddModule(testNameModule + 1, testDescriptionModule + 1)
                    .AddModule(testNameModule + 1, testDescriptionModule + 1);

                c.New(TestEnum.Test2, testDescription + 2).SetType(type)
                    .AddModule(testNameModule + 2, testDescriptionModule + 2)
                    .AddModule(testNameModule + 2, testDescriptionModule + 2);
            });

            var result = builder.Build();
            var firstElementCreated = result.ToArray()[0];
            var secondElementCreated = result.ToArray()[1];

            Assert.AreEqual(result.Count(), 2);

            Assert.AreEqual(firstElementCreated.Name, TestEnum.Test1.ToString());
            Assert.AreEqual(firstElementCreated.Description, testDescription + 1);
            Assert.AreEqual(firstElementCreated.Type, type);
            Assert.AreEqual(firstElementCreated.ModuleDefinitions.Count(), 2);

            Assert.AreEqual(secondElementCreated.Name, TestEnum.Test2.ToString());
            Assert.AreEqual(secondElementCreated.Description, testDescription + 2);
            Assert.AreEqual(secondElementCreated.Type, type);
            Assert.AreEqual(secondElementCreated.ModuleDefinitions.Count(), 2);
        }

        [TestMethod]
        public void Given_CreatedDefinitions_WhenTestNotAllEnumTyped_Then_ErrorAdded()
        {
            string testDescription = "TestDescription", testNameModule = "ModuleName", testDescriptionModule = "ModuleDescription";
            var type = ExecutionType.Automatic;
            var builder = new ExecutionBuilder<TestEnum, Guid>();

            builder.Generate(c =>
            {
                c.New(TestEnum.Test1, testDescription + 1).SetType(type)
                    .AddModule(testNameModule + 1, testDescriptionModule + 1)
                    .AddModule(testNameModule + 1, testDescriptionModule + 1);
            });

            Assert.AreEqual(builder.Errors.Count(), 1);
        }

    }

    public enum TestEnum
    {
        Test1, Test2
    }
}
