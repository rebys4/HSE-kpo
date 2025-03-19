using System;
using NUnit.Framework;
using HSE_Bank.Domain;
using HSE_Bank.Factories;

namespace HSE_Bank.Tests
{
    [TestFixture]
    public class DomainFactoryTests
    {
        [Test]
        public void CreateBankAccount_ShouldReturnValidAccount()
        {
            var account = DomainFactory.CreateBankAccount("Тестовый счет", 200m);
            Assert.IsNotNull(account);
            Assert.AreEqual("Тестовый счет", account.Name);
            Assert.AreEqual(200m, account.Balance);
        }

        [Test]
        public void CreateCategory_ShouldReturnValidCategory()
        {
            var category = DomainFactory.CreateCategory(CategoryType.Expense, "Тестовый расход");
            Assert.IsNotNull(category);
            Assert.AreEqual("Тестовый расход", category.Name);
            Assert.AreEqual(CategoryType.Expense, category.Type);
        }

        [Test]
        public void CreateOperation_ShouldThrowException_WhenAmountNegative()
        {
            var account = DomainFactory.CreateBankAccount("Счет", 1000m);
            var category = DomainFactory.CreateCategory(CategoryType.Expense, "Тест");
            Assert.Throws<ArgumentException>(() => 
                DomainFactory.CreateOperation(OperationType.Expense, account.Id, -100m, DateTime.Now, category.Id, "Неверная сумма"));
        }
    }
}