using System;
using Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class EmployeeFactoryTests
    {
        [TestMethod]
        public void CreateByPositionName()
        {
            //Arrange
            var service = new TechService();
            var factory = new EmployeeFactory(service);
            
            //Act
            var director = factory.CreateByPositionName("Director", "Директор");
            var manager = factory.CreateByPositionName("Manager", "Менеджер");
            var operator1 = factory.CreateByPositionName("Operator", "Оператор");

            //Assert
            Assert.AreEqual(3, service.Employees.Count);
            Assert.IsInstanceOfType(director, typeof(Director));
            Assert.IsInstanceOfType(manager, typeof(Manager));
            Assert.IsInstanceOfType(operator1, typeof(Operator));
        }
    }
}
