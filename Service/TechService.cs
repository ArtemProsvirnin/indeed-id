using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Service
{
    public class TechService
    {
        private TechServiceConfig _config;
        private ChainOfHandlers _chainOfHandlers;

        public TimeSpan MinTime {
            get => _config.TimeRange.MinTime;
            set => _config.TimeRange.MinTime = value;
        }

        public TimeSpan MaxTime
        {
            get => _config.TimeRange.MaxTime;
            set => _config.TimeRange.MaxTime = value;
        }

        public TimeSpan Td
        {
            get => _config.Td;
            set => _config.Td = value;
        }

        public TimeSpan Tm
        {
            get => _config.Tm;
            set => _config.Tm = value;
        }

        public Employees Employees { get; }
        public TasksManager TasksManager { get; }

        public TechService(TechServiceConfig config = null)
        {
            _config = config ?? new TechServiceConfig();
            Employees = new Employees();
            TasksManager = new TasksManager();

            CreateChainOfHandlers();//Создаем цепочку обработчиков заказов
            InfiniteWork();// Запускаем бесконечный цикл обработки запросов
        }

        private void CreateChainOfHandlers()
        {
            var operators = new ChainOfHandlers(Employees.Operators, TimeSpan.Zero);
            var managers = new ChainOfHandlers(Employees.Managers, Tm);
            var directors = new ChainOfHandlers(Employees.Directors, Td);            

            operators.Next = managers;
            managers.Next = directors;

            _chainOfHandlers = operators;
        }

        public void CreateTask(string description)
        {
            TasksManager.CreateTask(description);
        }

        private async void InfiniteWork()
        {
            while (true)
            {
                await DoWork();
                await Task.Delay(500);
            }
        }

        private async Task DoWork(TechTask task = null)
        {
            task = task ?? TasksManager.GetNextTask();

            if (task == null)
                return;

            await Task.Run(async () =>
            {
                if (!_chainOfHandlers.Process(task, _config.TimeRange))
                    await DoWork(task);
            });
        }
    }
}
