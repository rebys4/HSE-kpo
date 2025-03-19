using System;
using System.IO;
using NUnit.Framework;
using HSE_Bank.Domain;
using HSE_Bank.DataImportExport;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FinanceApp.Tests
{
    [TestFixture]
    public class ExportImportTests
    {
        private BankAccount testAccount;
        private Category testCategory;
        private Operation testOperation;

        [SetUp]
        public void Setup()
        {
            testAccount = new BankAccount(Guid.NewGuid(), "Test Account", 1000m);
            testCategory = new Category(Guid.NewGuid(), "Test Income", CategoryType.Income);
            testOperation = new Operation(Guid.NewGuid(), OperationType.Income, testAccount.Id, 500m, DateTime.Now, testCategory.Id, "Test operation");
        }

        [Test]
        public void JsonExport_SaveToFile_CreatesValidJsonArray()
        {
            string tempFile = Path.GetTempFileName();
            try
            {
                var exporter = new JsonExportVisitor(tempFile);
                exporter.ExportBankAccount(testAccount);
                exporter.ExportCategory(testCategory);
                exporter.ExportOperation(testOperation);
                exporter.SaveToFile();

                string content = File.ReadAllText(tempFile);
                JArray array = JArray.Parse(content);
                Assert.AreEqual(3, array.Count, "JSON-массив должен содержать 3 элемента.");
                
                JObject accountObj = (JObject)array[0];
                Assert.AreEqual(testAccount.Id.ToString(), accountObj["Id"]?.ToString());
            }
            finally
            {
                if(File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [Test]
        public void CsvExport_SaveToFile_CreatesFileWithHeaderAndData()
        {
            string tempFile = Path.GetTempFileName();
            try
            {
                var exporter = new CsvExportVisitor(tempFile);
                exporter.ExportBankAccount(testAccount);
                exporter.ExportCategory(testCategory);
                exporter.ExportOperation(testOperation);
                exporter.SaveToFile();

                string content = File.ReadAllText(tempFile);
                StringAssert.Contains("Type,Id,Name,Balance,OperationType,BankAccountId,Amount,Date,CategoryId,Description", content);
                StringAssert.Contains("BankAccount", content);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [Test]
        public void YamlExport_SaveToFile_CreatesValidYaml()
        {
            string tempFile = Path.GetTempFileName();
            try
            {
                var exporter = new YamlExportVisitor(tempFile);
                exporter.ExportBankAccount(testAccount);
                exporter.ExportCategory(testCategory);
                exporter.ExportOperation(testOperation);
                exporter.SaveToFile();

                string content = File.ReadAllText(tempFile);
                StringAssert.Contains("id:", content.ToLower());
                StringAssert.Contains("name:", content.ToLower());
                
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                var yamlData = deserializer.Deserialize<object>(content);
                Assert.IsNotNull(yamlData, "Десериализованные данные из YAML не должны быть null");
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [Test]
        public void JsonImport_ValidJsonFile_ShouldImportData()
        {
            var accountJson = Newtonsoft.Json.JsonConvert.SerializeObject(testAccount, Newtonsoft.Json.Formatting.Indented);
            string jsonContent = "[\n" + accountJson + "\n]";
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, jsonContent);

            var importer = new JsonDataImporter();
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                importer.Import(tempFile);
                string output = sw.ToString();
                StringAssert.Contains("Импортированы данные", output);
            }
            File.Delete(tempFile);
        }

        [Test]
        public void CsvImport_ValidCsvFile_ShouldImportData()
        {
            string csvContent = "Header1,Header2\nValue1,Value2";
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, csvContent);

            var importer = new CsvDataImporter();
            using(var sw = new StringWriter())
            {
                Console.SetOut(sw);
                importer.Import(tempFile);
                string output = sw.ToString();
                StringAssert.Contains("Импортировано", output);
            }
            File.Delete(tempFile);
        }

        [Test]
        public void YamlImport_ValidYamlFile_ShouldImportData()
        {
            string yamlContent = @"
                data:
                  - id: 1
                    name: TestName
                  - id: 2
                    name: TestName2
            ";
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, yamlContent);

            var importer = new YamlDataImporter();
            using(var sw = new StringWriter())
            {
                // Перенаправляем стандартный вывод
                Console.SetOut(sw);
                importer.Import(tempFile);
                string output = sw.ToString();
                StringAssert.Contains("Импортированы данные", output);
            }
            File.Delete(tempFile);
        }
    }
}
