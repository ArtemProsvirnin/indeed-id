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
            
            //Act
            var director = service.CreateEmployee("Director", "Директор");
            var manager = service.CreateEmployee("Manager", "Менеджер");
            var operator1 = service.CreateEmployee("Operator", "Оператор");

            //Assert
            Assert.AreEqual(3, service.Employees.Count);
            Assert.IsInstanceOfType(director, typeof(Director));
            Assert.IsInstanceOfType(manager, typeof(Manager));
            Assert.IsInstanceOfType(operator1, typeof(Operator));
        }
    }
}
