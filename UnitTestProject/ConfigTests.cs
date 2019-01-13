using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class ConfigTests
    {

        [TestMethod]
        public void ChangeTimeRange()
        {
            //Arrange
            var config = new TechServiceConfig
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0))
            };

            var service = new TechService(config);

            //Act
            service.CreateTask("Запрос в службу поддержки №1");

            service.CreateOperator("Оператор");

            service.MinTime = TimeSpan.FromSeconds(3);
            service.MaxTime = TimeSpan.FromSeconds(3);

            service.CreateTask("Запрос в службу поддержки №2");

            //Assert
            var task = Task.Run(async () => {
                //Ждем выполнения всех заявок
                while (!service.TaskManager.AllDone)
                {
                    await Task.Delay(500);
                }

                var firstTask = service.TaskManager.Done.First();
                var secondTask = service.TaskManager.Done.Skip(1).First();

                Assert.IsTrue(((int)firstTask.TimeSpent.TotalSeconds + (int)service.MaxTime.TotalSeconds) == (int)secondTask.TimeSpent.TotalSeconds);
            });

            task.Wait();
        }

        [TestMethod]
        public void ChangeTm()
        {
            //Arrange
            var config = new TechServiceConfig
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0)),
                Tm = TimeSpan.FromSeconds(0)
            };

            var service = new TechService(config);
            
            //Act
            service.CreateTask("Запрос в службу поддержки №1");

            service.CreateManager("Менеджер");

            service.Tm = TimeSpan.FromSeconds(3);

            service.CreateTask("Запрос в службу поддержки №2");

            //Assert
            var task = Task.Run(async () => {
                //Ждем выполнения всех заявок
                while (!service.TaskManager.AllDone)
                {
                    await Task.Delay(500);
                }

                var firstTask = service.TaskManager.Done.First();
                var secondTask = service.TaskManager.Done.Skip(1).First();

                Assert.IsTrue(((int)firstTask.TimeSpent.TotalSeconds + (int)service.Tm.TotalSeconds) == (int)secondTask.TimeSpent.TotalSeconds);
            });

            task.Wait();
        }

        [TestMethod]
        public void ChangeTd()
        {
            //Arrange
            var config = new TechServiceConfig
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0)),
                Td = TimeSpan.FromSeconds(0)
            };

            var service = new TechService(config);

            //Act
            service.CreateTask("Запрос в службу поддержки №1");

            service.CreateDirector("Директор");

            service.Td = TimeSpan.FromSeconds(3);

            service.CreateTask("Запрос в службу поддержки №2");

            //Assert

            var task = Task.Run(async () => {
                //Ждем выполнения всех заявок
                while (!service.TaskManager.AllDone)
                {
                    await Task.Delay(500);
                }

                var firstTask = service.TaskManager.Done.First();
                var secondTask = service.TaskManager.Done.Skip(1).First();

                Assert.IsTrue(((int)firstTask.TimeSpent.TotalSeconds + (int)service.Td.TotalSeconds) == (int)secondTask.TimeSpent.TotalSeconds);
            });

            task.Wait();
        }
    }
}
