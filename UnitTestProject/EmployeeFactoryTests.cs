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
            service.CreateTask("Запрос в службу поддержки №1");
            var operator1 = service.CreateEmployee("Operator", "Оператор");

            //Assert
            Task.Run(() =>
            {
                Assert.AreEqual(0, service.TaskManager.InQueue.Count());
                Assert.AreEqual(1, service.TaskManager.InWork.Count());

                service.DeleteEmployee(operator1);

                Assert.AreEqual(1, service.TaskManager.InQueue.Count());
                Assert.AreEqual(0, service.TaskManager.InWork.Count());
            });
        }
    }
}
