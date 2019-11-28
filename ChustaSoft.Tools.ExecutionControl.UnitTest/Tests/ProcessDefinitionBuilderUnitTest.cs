using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.ExecutionControl.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChustaSoft.Tools.ExecutionControl.UnitTest
{
    [TestClass]
    public class ProcessDefinitionBuilderUnitTest
    {

        [TestMethod]
        public void Given_SingleEnumTypeWithoutDescription_When_Add_Then_ProcessDefinitionWithSameNameAndDescriptionCreated()
        {
            var builder = new ProcessDefinitionBuilder<int, TestUndefinedProcesses>();

            builder.Add(TestUndefinedProcesses.Process1);

            Assert.AreEqual(builder.Count, 1);
            Assert.AreEqual(builder.Definitions.First().Description, TestUndefinedProcesses.Process1.ToString());
            Assert.AreEqual(builder.Definitions.First().Name, TestUndefinedProcesses.Process1.ToString());
        }

        [TestMethod]
        public void Given_FullEnumTypeWithoutDescriptions_When_Add_Then_AllProcessDefinitionWithSameNameAndDescriptionCreated()
        {
            var builder = new ProcessDefinitionBuilder<int, TestUndefinedProcesses>();
            var expectedList = EnumsHelper.GetEnumList<TestUndefinedProcesses>();
            builder.Autogenerate();

            Assert.AreEqual(builder.Count, expectedList.Count());
            Assert.IsTrue(builder.Definitions.Any(x => expectedList.Contains(EnumsHelper.GetByString<TestUndefinedProcesses>(x.Name))));
            Assert.IsTrue(builder.Definitions.Any(x => expectedList.Contains(EnumsHelper.GetByString<TestUndefinedProcesses>(x.Description))));
        }

        [TestMethod]
        public void Given_SingleEnumTypeWithDescription_When_Add_Then_ProcessDefinitionWithNameAndDescriptionCreated()
        {
            var builder = new ProcessDefinitionBuilder<int, TestDefinedProcesses>();

            builder.Add(TestDefinedProcesses.Process1);

            Assert.AreEqual(builder.Count, 1);
            Assert.AreEqual(builder.Definitions.First().Description, TestDefinedProcesses.Process1.GetDescription());
            Assert.AreEqual(builder.Definitions.First().Name, TestDefinedProcesses.Process1.ToString());
        }

        [TestMethod]
        public void Given_FullEnumTypeWithDescriptions_When_Add_Then_AllProcessDefinitionWithNameAndDescriptionCreated()
        {
            var builder = new ProcessDefinitionBuilder<int, TestUndefinedProcesses>();
            var expectedList = EnumsHelper.GetEnumList<TestUndefinedProcesses>();
            builder.Autogenerate();

            Assert.AreEqual(builder.Count, expectedList.Count());
            Assert.IsTrue(builder.Definitions.Any(x => expectedList.Contains(EnumsHelper.GetByString<TestUndefinedProcesses>(x.Name))));
            Assert.IsTrue(builder.Definitions.Any(x => expectedList.Contains(EnumsHelper.GetByDescription<TestUndefinedProcesses>(x.Description))));
        }

        [TestMethod]
        public void Given_EnumWithProcessDefinitionAttributes_When_Add_Then_AllProcessDefinitionWithNameDescriptionAndTypeCreated()
        {
            var builder = new ProcessDefinitionBuilder<int, TestExtraDefinedProcesses>();
            var expectedList = EnumsHelper.GetEnumList<TestExtraDefinedProcesses>();
            builder.Autogenerate();

            Assert.AreEqual(builder.Count, expectedList.Count());
            Assert.IsTrue(builder.Definitions.Any(x => expectedList.Contains(EnumsHelper.GetByString<TestExtraDefinedProcesses>(x.Name))));
            Assert.IsTrue(builder.Definitions.Any(x => expectedList.Contains(EnumsHelper.GetByDescription<TestExtraDefinedProcesses>(x.Description))));
            Assert.IsTrue(builder.Definitions.Any(x => x.Type == Enums.ProcessType.Background));
            Assert.IsTrue(builder.Definitions.Any(x => x.Type == Enums.ProcessType.Batch));
        }

    }
}
