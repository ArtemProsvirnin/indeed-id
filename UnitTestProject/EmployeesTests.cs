using Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestProject
{
    [TestClass]
    public class EmployeesTests
    {
        [TestMethod]
        public void CreateEmployees()
        {
            //Arrange
            Mock<ITaskManager> mock = new Mock<ITaskManager>();
            Employees employees = new Employees(mock.Object);

            //Act
            var director = employees.CreateDirector("Директор");
            var manager = employees.CreateManager("Менеджер");
            var operator1 = employees.CreateOperator("Оператор №1");
            var operator2 = employees.CreateOperator("Оператор №2");

            //Assert
            Assert.AreEqual(4, employees.Count);
            Assert.AreEqual(1, employees.Directors.Count);
            Assert.AreEqual(1, employees.Managers.Count);
            Assert.AreEqual(2, employees.Operators.Count);
            Assert.IsInstanceOfType(director, typeof(Director));
            Assert.IsInstanceOfType(manager, typeof(Manager));
            Assert.IsInstanceOfType(operator1, typeof(Operator));
            Assert.IsInstanceOfType(operator2, typeof(Operator));
        }

        [TestMethod]
        public void CreateByPositionName()
        {
            //Arrange
            Mock<ITaskManager> mock = new Mock<ITaskManager>();
            Employees employees = new Employees(mock.Object);

            //Act
            var director = employees.CreateByPositionName("Director", "Директор");
            var manager = employees.CreateByPositionName("Manager", "Менеджер");
            var operator1 = employees.CreateByPositionName("Operator", "Оператор");

            //Assert
            Assert.AreEqual(3, employees.Count);
            Assert.IsInstanceOfType(director, typeof(Director));
            Assert.IsInstanceOfType(manager, typeof(Manager));
            Assert.IsInstanceOfType(operator1, typeof(Operator));
        }

        [TestMethod]
        public void DeleteEmployee()
        {
            //Arrange
            Mock<ITaskManager> mock = new Mock<ITaskManager>();
            Employees employees = new Employees(mock.Object);

            //Act
            var director = employees.CreateDirector("Директор");
            employees.Remove(director);

            //Assert
            Assert.AreEqual(0, employees.Count);
        }

        [TestMethod]
        public void DeleteEmployeeById()
        {
            //Arrange
            Mock<ITaskManager> mock = new Mock<ITaskManager>();
            Employees employees = new Employees(mock.Object);

            //Act
            employees.CreateDirector("Директор");
            employees.Remove(0);

            //Assert
            Assert.AreEqual(0, employees.Count);
        }
    }
}
