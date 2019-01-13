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

                var firstTime = (int)firstTask.TimeSpent.TotalSeconds + (int)service.MaxTime.TotalSeconds;
                var secondTime = (int)secondTask.TimeSpent.TotalSeconds;

                Assert.IsTrue(firstTime == secondTime);
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
            var newTm = TimeSpan.FromSeconds(3);
            service.Tm = newTm;

            //Assert
            Assert.AreEqual(service.TaskManager.Config.Tm, newTm);
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
            var newTd = TimeSpan.FromSeconds(3);
            service.Td = newTd;

            service.CreateTask("Запрос в службу поддержки №2");

            //Assert
            Assert.AreEqual(service.TaskManager.Config.Td, newTd);
        }
    }
}
