using CheckoutKata;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
    {
        private IDataRepository _dataRepository;

        [SetUp]
        public void Setup()
        {
            var service = new Mock<IDataRepository>();
            service.Setup(m => m.GetItemPrice("A")).Returns(() => 50);
            service.Setup(m => m.GetItemPrice("B")).Returns(() => 30);
            service.Setup(m => m.GetItemPrice("C")).Returns(() => 20);
            service.Setup(m => m.GetItemPrice("D")).Returns(() => 15);
            _dataRepository = service.Object;
        }

        /// <summary>
        /// This first test is to scan a single item and ensure the correct price is returned as the total.
        /// </summary>
        /// <param name="item">The </param>
        /// <param name="expected"></param>
        [Test]
        [TestCase("A", 50)]
        [TestCase("B", 30)]
        [TestCase("C", 20)]
        [TestCase("D", 15)]
        public void Scan1ItemTotal(string item, int expected)
        {
            //arrange
            var checkout = new Checkout(_dataRepository);
            //act
            checkout.Scan(item);
            var price = checkout.GetTotalPrice();
            //assert 
            Assert.AreEqual(expected, price);
        }

        /// <summary>
        /// Tests passing in an invalid SKU into the scan function
        /// </summary>
        [Test]
        public void ScanInvalidItem()
        {
            //arrange
            var checkout = new Checkout(_dataRepository);
            //act
            checkout.Scan("E");
            //assert
            Assert.Throws<ArgumentException>(() => checkout.GetTotalPrice());
        }

        /// <summary>
        /// Test passing in null to the scan function
        /// </summary>
        [Test]
        public void ScanNullItem()
        {
            //arrange
            var checkout = new Checkout(_dataRepository);
            //assert
            Assert.Throws<ArgumentNullException>(() => checkout.Scan(null));
        }

        /// <summary>
        /// Tests the price is calculated correctly with multiple items.  These combinations do not include any special prices
        /// A test should also be added that adds enough items so that GetTotalPrice reaches int.max
        /// </summary>
        /// <param name="items">the items to scan</param>
        /// <param name="expected">expected price of the items</param>
        [Test]
        [TestCase(new string[] { "A", "B" }, 80)]
        [TestCase(new string[] { "A", "B", "C" }, 100)]
        [TestCase(new string[] { "A", "B", "C", "D" }, 115)]
        [TestCase(new string[] { "A", "B", "C", "C", "C", "A", "D", "C" }, 225)]
        public void ScanMultipleItems_WithoutSpecialPrice(string[] items, int expected)
        {
            //arrange
            var checkout = new Checkout(_dataRepository);
            //act
            foreach (var item in items)
            {
                checkout.Scan(item);
            }
            var totalPrice = checkout.GetTotalPrice();
            //assert
            Assert.AreEqual(expected, totalPrice);
        }

        /// <summary>
        /// Test to get price with 0 items scanned
        /// </summary>
        [Test]
        public void GetTotalPrice_NoItems()
        {
            //arrange
            var checkout = new Checkout(_dataRepository);
            //act
            var price = checkout.GetTotalPrice();
            //assert 
            Assert.AreEqual(0, price);
        }

        /// <summary>
        /// Scan multiple items that include special price deals.
        /// This could actually be done in the same test ScanMultipleItems_WithoutSpecialPrice but with different items passed.  
        /// However I have it seperate so the test results are clearer.
        /// </summary>
        /// <param name="items">The items to scan</param>
        /// <param name="expected">expected total price</param>
        [Test]
        [TestCase(new string[] { "A", "A", "A" }, 130)]
        [TestCase(new string[] { "B", "B" }, 45)]
        [TestCase(new string[] { "A", "B", "A", "B", "A" }, 175)]
        [TestCase(new string[] { "A", "C", "A", "A" }, 150)]
        [TestCase(new string[] { "A", "A", "A", "A" }, 180)]
        [TestCase(new string[] { "A", "A", "A", "A", "A", "A" }, 260)]
        public void ScanMultipleItems_WithSpecialPrice(string[] items, int expected)
        {
            //arrange
            var checkout = new Checkout(_dataRepository);
            //act
            foreach (var item in items)
            {
                checkout.Scan(item);
            }
            var totalPrice = checkout.GetTotalPrice();
            //assert
            Assert.AreEqual(expected, totalPrice);
        }
    }
}