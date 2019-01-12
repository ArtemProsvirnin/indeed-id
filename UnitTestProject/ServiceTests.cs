using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void CreateTechService()
        {
            //Arrange
            var config = new TechServiceConfig
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5)),
                Tm = TimeSpan.FromSeconds(10),
                Td = TimeSpan.FromSeconds(20)
            };

            //Act
            var service = new TechService(config);

            //Assert
            Assert.AreEqual(0, service.Employees.Count);
            Assert.AreEqual(0, service.TasksManager.InWork.Count());
            Assert.AreEqual(0, service.TasksManager.Done.Count());
            Assert.AreEqual(0, service.TasksManager.InQueue.Count());
            Assert.AreEqual(TimeSpan.FromSeconds(2), service.MinTime);
            Assert.AreEqual(TimeSpan.FromSeconds(5), service.MaxTime);
            Assert.AreEqual(TimeSpan.FromSeconds(10), service.Tm);
            Assert.AreEqual(TimeSpan.FromSeconds(20), service.Td);
        }

        [TestMethod]
        public void CreateEmployees()
        {
            //Arrange
            var service = new TechService();
            var factory = new EmployeeFactory(service);
            //Act
            var director = factory.CreateDirector("Директор");
            var manager = factory.CreateManager("Менеджер");
            var operator1 = factory.CreateOperator("Оператор #1");
            var operator2 = factory.CreateOperator("Оператор #2");

            //Assert
            Assert.AreEqual(4, service.Employees.Count);
            Assert.AreEqual(1, service.Employees.Directors.Count);
            Assert.AreEqual(1, service.Employees.Managers.Count);
            Assert.AreEqual(2, service.Employees.Operators.Count);
        }

        [TestMethod]
        public void CreateTasks()
        {
            //Arrange
            var service = new TechService();
            //Act
            service.CreateTask("Запрос в службу поддержки #1");
            service.CreateTask("Запрос в службу поддержки #2");
            service.CreateTask("Запрос в службу поддержки #3");
            //Assert
            Assert.AreEqual(3, service.TasksManager.InQueue.Count());
            Assert.AreEqual(0, service.TasksManager.InWork.Count());
            Assert.AreEqual(0, service.TasksManager.Done.Count());
        }

        [TestMethod]
        public void OperatorHandlingTask()
        {
            //Arrange
            var config = new TechServiceConfig
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            };

            var service = new TechService(config);
            var factory = new EmployeeFactory(service);

            Employee operator1 = factory.CreateOperator("Оператор");

            //Act
            service.CreateTask("Запрос в службу поддержки");

            //Assert
            var t = Task.Run(async () => {
                //Подождем чтобы оператор успел обработать заявку
                await Task.Delay(2000);

                Assert.AreEqual(0, service.TasksManager.InQueue.Count());
                Assert.AreEqual(0, service.TasksManager.InWork.Count());
                Assert.AreEqual(1, service.TasksManager.Done.Count());
                Assert.AreEqual(operator1, service.TasksManager.Done.First().Handler);
            });

            Task.WaitAll(t);
        }

        [TestMethod]
        public void ManagerHandlingTask()
        {
            //Arrange
            var config = new TechServiceConfig
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(3)),
                Tm = TimeSpan.FromSeconds(2),
            };

            var service = new TechService(config);
            var factory = new EmployeeFactory(service);

            Employee manager = factory.CreateManager("Менеджер");
            Employee operator1 = factory.CreateOperator("Оператор");

            //Act
            service.CreateTask("Запрос в службу поддержки #1");
            service.CreateTask("Запрос в службу поддержки #2");

            //Assert
            var t = Task.Run(async () => {
                //Подождем чтобы оператор и менеджер успел обработать заявку
                await Task.Delay(6000);

                Assert.AreEqual(0, service.TasksManager.InQueue.Count());
                Assert.AreEqual(0, service.TasksManager.InWork.Count());
                Assert.AreEqual(1, service.TasksManager.Done.Count());
            });

            Task.WaitAll(t);
        }
    }
}
