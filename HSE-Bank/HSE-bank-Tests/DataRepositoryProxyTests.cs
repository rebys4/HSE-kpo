using System;
using NUnit.Framework;
using HSE_Bank.Domain;
using HSE_Bank.Repositories;

namespace HSE_Bank.Tests
{
    [TestFixture]
    public class DataRepositoryProxyTests
    {
        private DataRepositoryProxy _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new DataRepositoryProxy();
        }

        [Test]
        public void AddAndGetBankAccount_ShouldReturnCorrectAccount()
        {
            var account = new BankAccount(Guid.NewGuid(), "Счет тест", 100m);
            _repository.AddBankAccount(account);

            var retrieved = _repository.GetBankAccount(account.Id);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("Счет тест", retrieved.Name);
            Assert.AreEqual(100m, retrieved.Balance);
        }

        [Test]
        public void UpdateBankAccount_ShouldModifyAccountData()
        {
            var account = new BankAccount(Guid.NewGuid(), "Счет тест", 100m);
            _repository.AddBankAccount(account);
            account.SetName("Обновленный счет");
            account.UpadteBalance(50m);
            _repository.UpdateBankAccount(account);

            var updated = _repository.GetBankAccount(account.Id);
            Assert.AreEqual("Обновленный счет", updated.Name);
            Assert.AreEqual(150m, updated.Balance);
        }

        [Test]
        public void DeleteBankAccount_ShouldRemoveAccount()
        {
            var account = new BankAccount(Guid.NewGuid(), "Счет тест", 100m);
            _repository.AddBankAccount(account);
            _repository.DeleteBankAccount(account.Id);

            var deleted = _repository.GetBankAccount(account.Id);
            Assert.IsNull(deleted);
        }
    }
}