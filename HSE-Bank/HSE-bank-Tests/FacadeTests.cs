using System;
using System.Linq;
using HSE_Bank.Domain;
using HSE_Bank.Facades;
using HSE_Bank.Repositories;
using NUnit.Framework;

namespace HSE_Bank.Tests
{
    [TestFixture]
    public class FacadeTests
    {
        private IDataRepository _repository;
        private Facade _facade;

        [SetUp]
        public void Setup()
        {
            _repository = new DataRepositoryProxy();
            _facade = new Facade(_repository);
        }

        [Test]
        public void CreateAccount_ShouldAddAccountToRepository()
        {
            var account = _facade.CreateBankAccount("Тестовый счет", 500m);
            
            Assert.IsNotNull(account);
            var retrieved = _repository.GetBankAccount(account.Id);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("Тестовый счет", retrieved.Name);
            Assert.AreEqual(500m, retrieved.Balance);
        }

        [Test]
        public void CreateOperation_ShouldUpdateAccountBalance_ForIncomeOperation()
        {
            var account = _facade.CreateBankAccount("Счет", 1000m);
            var category = _facade.CreateCategory(CategoryType.Income, "Зарплата");
            
            _facade.CreateOperation(OperationType.Income, account.Id, 500m, DateTime.Now, category.Id, "Тестовая операция");
            var updatedAccount = _repository.GetBankAccount(account.Id);
            
            Assert.AreEqual(1500m, updatedAccount.Balance);
        }

        [Test]
        public void CreateOperation_ShouldUpdateAccountBalance_ForExpenseOperation()
        {
            var account = _facade.CreateBankAccount("Счет", 1000m);
            var category = _facade.CreateCategory(CategoryType.Expense, "Кафе");
            
            _facade.CreateOperation(OperationType.Expense, account.Id, 300m, DateTime.Now, category.Id, "Тестовая операция");
            var updatedAccount = _repository.GetBankAccount(account.Id);
            
            Assert.AreEqual(700m, updatedAccount.Balance);
        }

        [Test]
        public void CalculateNetAmount_ShouldReturnCorrectNetAmount()
        {
            var account = _facade.CreateBankAccount("Счет", 0);
            var incomeCategory = _facade.CreateCategory(CategoryType.Income, "Зарплата");
            var expenseCategory = _facade.CreateCategory(CategoryType.Expense, "Кафе");
            
            var start = DateTime.Now.AddDays(-1);
            var end = DateTime.Now.AddDays(1);
            _facade.CreateOperation(OperationType.Income, account.Id, 1000m, DateTime.Now, incomeCategory.Id, "Доход");
            _facade.CreateOperation(OperationType.Expense, account.Id, 400m, DateTime.Now, expenseCategory.Id, "Расход");
            
            var net = _facade.CalculateNewBalance(start, end);
            
            Assert.AreEqual(600m, net);
        }

        [Test]
        public void GroupOperationsByCategory_ShouldReturnCorrectGrouping()
        {
            var account = _facade.CreateBankAccount("Счет", 0);
            var incomeCategory = _facade.CreateCategory(CategoryType.Income, "Зарплата");
            var expenseCategory = _facade.CreateCategory(CategoryType.Expense, "Кафе");
            
            _facade.CreateOperation(OperationType.Income, account.Id, 1000m, DateTime.Now, incomeCategory.Id, "Доход 1");
            _facade.CreateOperation(OperationType.Income, account.Id, 500m, DateTime.Now, incomeCategory.Id, "Доход 2");
            _facade.CreateOperation(OperationType.Expense, account.Id, 300m, DateTime.Now, expenseCategory.Id, "Расход 1");
            
            var grouped = _facade.GroupOperationsByCategory(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
            
            Assert.IsTrue(grouped.ContainsKey(incomeCategory.Id));
            Assert.IsTrue(grouped.ContainsKey(expenseCategory.Id));
            Assert.AreEqual(1500m, grouped[incomeCategory.Id]);
            Assert.AreEqual(-300m, grouped[expenseCategory.Id]);
        }
    }
}
