using System;
using Service;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class EmployeeFactoryTests
    {
        [TestMethod]
        public void CreateEmployees()
        {
            //Arrange
            var service = new TechService();
            //Act
            var director = service.CreateDirector("Директор");
            var manager = service.CreateManager("Менеджер");
            var operator1 = service.CreateOperator("Оператор №1");
            var operator2 = service.CreateOperator("Оператор №2");

            //Assert
            Assert.AreEqual(4, service.Employees.Count);
            Assert.AreEqual(1, service.Employees.Directors.Count);
            Assert.AreEqual(1, service.Employees.Managers.Count);
            Assert.AreEqual(2, service.Employees.Operators.Count);
            Assert.IsInstanceOfType(director, typeof(Director));
            Assert.IsInstanceOfType(manager, typeof(Manager));
            Assert.IsInstanceOfType(operator1, typeof(Operator));
            Assert.IsInstanceOfType(operator2, typeof(Operator));
        }

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

        [TestMethod]
        public void DeleteEmployee()
        {
            //Arrange
            var service = new TechService();

            //Act
            var director = service.CreateDirector("Директор");
            service.Employees.Remove(director);

            //Assert
            Assert.AreEqual(0, service.Employees.Count);
        }
    }
}
