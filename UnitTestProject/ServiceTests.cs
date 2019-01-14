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
            Assert.AreEqual(0, service.TaskManager.InWork.Count());
            Assert.AreEqual(0, service.TaskManager.Done.Count());
            Assert.AreEqual(0, service.TaskManager.InQueue.Count());
            Assert.AreEqual(TimeSpan.FromSeconds(2), service.MinTime);
            Assert.AreEqual(TimeSpan.FromSeconds(5), service.MaxTime);
            Assert.AreEqual(TimeSpan.FromSeconds(10), service.Tm);
            Assert.AreEqual(TimeSpan.FromSeconds(20), service.Td);
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
            var task = Task.Run(async () =>
            {
                //Ждем пока оператор не получит задачу
                while (service.TaskManager.InWork.Count() != 1)
                    await Task.Delay(500);

                service.DeleteEmployee(operator1);

                //Ждем пока оператор не вернет задачу
                while (service.TaskManager.InQueue.Count() != 1)
                {
                    Assert.AreEqual(0, service.TaskManager.Done.Count());
                    await Task.Delay(500);
                }

                Assert.AreEqual(0, service.TaskManager.InWork.Count());
            });

            task.Wait();
        }
        
        [TestMethod]
        public void CreateTasks()
        {
            //Arrange
            var service = new TechService();
            //Act
            service.CreateTask("Запрос в службу поддержки №1");
            service.CreateTask("Запрос в службу поддержки №2");
            service.CreateTask("Запрос в службу поддержки №3");
            //Assert
            Assert.AreEqual(3, service.TaskManager.InQueue.Count());
            Assert.AreEqual(0, service.TaskManager.InWork.Count());
            Assert.AreEqual(0, service.TaskManager.Done.Count());
        }

        [TestMethod]
        public void DeleteTask()
        {
            //Arrange
            var service = new TechService();
            //Act
            TechTask task = service.CreateTask("Запрос в службу поддержки №1");
            service.DeleteTask(task);
            //Assert
            Assert.AreEqual(TechTaskStatus.Canceled, task.Status);
        }

        [TestMethod]
        public void OperatorHandlingTask()
        {
            //Arrange
            var config = new TechServiceConfig
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(1))
            };

            var service = new TechService(config);

            //Act
            service.CreateTask("Запрос в службу поддержки");

            Employee operator1 = service.CreateOperator("Оператор");

            //Assert
            var task = Task.Run(async () => {
                //Ждем выполнения всех заявок
                while (!service.TaskManager.AllDone)
                    await Task.Delay(500);

                Assert.AreEqual(0, service.TaskManager.InQueue.Count());
                Assert.AreEqual(0, service.TaskManager.InWork.Count());
                Assert.AreEqual(1, service.TaskManager.Done.Count());
                Assert.AreEqual(operator1, service.TaskManager.Done.First().Handler);
                Assert.AreEqual(TechTaskStatus.Done, service.TaskManager.Done.First().Status);
                Assert.IsFalse(operator1.IsBusy);
            });

            task.Wait();
        }

        [TestMethod]
        public void ManagerHandlingTask()
        {
            //Arrange
            var config = new TechServiceConfig
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(2)),
                Tm = TimeSpan.FromSeconds(0),
            };

            var service = new TechService(config);

            //Act
            service.CreateTask("Запрос в службу поддержки №1");
            service.CreateTask("Запрос в службу поддержки №2 (для менеджера)");

            Employee operator1 = service.CreateOperator("Оператор");
            Employee manager = service.CreateManager("Менеджер");

            //Assert
            var task = Task.Run(async () => {
                //Ждем выполнения всех заявок
                while (!service.TaskManager.AllDone)
                {
                    await Task.Delay(500);
                }

                Assert.AreEqual(0, service.TaskManager.InQueue.Count());
                Assert.AreEqual(0, service.TaskManager.InWork.Count());
                Assert.AreEqual(2, service.TaskManager.Done.Count());
                Assert.AreEqual(operator1, service.TaskManager.Done.First(t => t.Description == "Запрос в службу поддержки №1").Handler);
                Assert.AreEqual(manager, service.TaskManager.Done.First(t => t.Description == "Запрос в службу поддержки №2 (для менеджера)").Handler);
            });

            task.Wait();
        }

        [TestMethod]
        public void DirectorHandlingTask()
        {
            //Arrange
            var config = new TechServiceConfig
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(3)),
                Tm = TimeSpan.FromSeconds(0),
                Td = TimeSpan.FromSeconds(1),
            };

            var service = new TechService(config);

            //Act
            service.CreateTask("Запрос в службу поддержки №1");
            service.CreateTask("Запрос в службу поддержки №2 (для менеджера)");
            service.CreateTask("Запрос в службу поддержки №3 (для директора)");

            Employee operator1 = service.CreateOperator("Оператор");
            Employee manager = service.CreateManager("Менеджер");
            Employee director = service.CreateDirector("Директор");

            //Assert
            var task = Task.Run(async () => {
                //Ждем выполнения всех заявок
                while (!service.TaskManager.AllDone)
                    await Task.Delay(500);

                Assert.AreEqual(0, service.TaskManager.InQueue.Count());
                Assert.AreEqual(0, service.TaskManager.InWork.Count());
                Assert.AreEqual(3, service.TaskManager.Done.Count());
                Assert.AreEqual(operator1, service.TaskManager.Done.First(t => t.Description == "Запрос в службу поддержки №1").Handler);
                Assert.AreEqual(manager, service.TaskManager.Done.First(t => t.Description == "Запрос в службу поддержки №2 (для менеджера)").Handler);
                Assert.AreEqual(director, service.TaskManager.Done.First(t => t.Description == "Запрос в службу поддержки №3 (для директора)").Handler);
            });

            task.Wait();
        }
    }
}
