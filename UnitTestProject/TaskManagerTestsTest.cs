using Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class TaskManagerTestsTest
    {
        [TestMethod]
        public void CreateTasks()
        {
            //Arrange
            ITaskManager manager = new TaskManager(new TechServiceConfig());
            
            //Act
            manager.CreateTask("Запрос в службу поддержки №1");
            manager.CreateTask("Запрос в службу поддержки №2");
            manager.CreateTask("Запрос в службу поддержки №3");
            
            //Assert
            Assert.AreEqual(3, manager.InQueue.Count);
            Assert.AreEqual(0, manager.InWork.Count);
            Assert.AreEqual(0, manager.Done.Count);
        }

        [TestMethod]
        public void DeleteTask()
        {
            //Arrange
            ITaskManager manager = new TaskManager(new TechServiceConfig());
            
            //Act
            TechTask techTask = manager.CreateTask("Запрос в службу поддержки №1");
            manager.DeleteTask(techTask);
            
            //Assert
            var task = Task.Run(async () =>
            {
                //Ждем когда очередь выбросит запрос
                while (manager.InQueue.Count != 0)
                    await Task.Delay(500);

                Assert.AreEqual(TechTaskStatus.Canceled, techTask.Status);
                Assert.AreEqual(0, manager.All.Count);
            });

            task.Wait();
        }
    }
}
