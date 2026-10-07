using System;
using ACM.BL;
using CMS.BusinessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMS.BusinessLayerTest
{
    [TestClass]
    public class RefactoringTest
    {
        [TestMethod]
        public void RepositoriesKeepRequestedIds()
        {
            Assert.AreEqual(12, new CustomerRepository().Retrieve(12).CustomerId);
            Assert.AreEqual(23, new ProductRepository().Retrieve(23).ProductId);
            Assert.AreEqual(34, new OrderRepository().Retrieve(34).OrderId);
            Assert.AreEqual(45, new OrderItemRepository().Retrieve(45).OrderItemId);
        }

        [TestMethod]
        public void CustomerAndOrderCanShareAnAddress()
        {
            var home = new Address { StreetLine1 = "Лесная, 10", City = "Минск" };
            var work = new Address { StreetLine1 = "Центральная, 20", City = "Минск" };
            var customer = new Customer(1) { HomeAddress = home, WorkAddress = work };
            var order = new Order(2) { Customer = customer, ShippingAddress = home };

            Assert.AreSame(customer, order.Customer);
            Assert.AreSame(customer.HomeAddress, order.ShippingAddress);
            Assert.AreNotSame(customer.HomeAddress, customer.WorkAddress);
            Assert.AreEqual("Центральная, 20", customer.WorkAddress.StreetLine1);
        }

        [TestMethod]
        public void ValidationStillRejectsIncompleteObjects()
        {
            Assert.IsFalse(new Customer().Validate());
            Assert.IsFalse(new Product().Validate());
            Assert.IsFalse(new Order().Validate());
            Assert.IsFalse(new OrderItem().Validate());
        }

        [TestMethod]
        public void ValidationStillAcceptsCompleteObjects()
        {
            Assert.IsTrue(new Customer { LastName = "Иванов", EmailAddress = "student@example.com" }.Validate());
            Assert.IsTrue(new Product { ProductName = "Книга", CurrentPrice = 10m }.Validate());
            Assert.IsTrue(new Order { OrderDate = DateTimeOffset.Now }.Validate());
            Assert.IsTrue(new OrderItem { ProductId = 1, OrderQuantity = 2, PurchasePrice = 10m }.Validate());
        }
    }
}
