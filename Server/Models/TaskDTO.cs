using Service;

namespace Server.Models
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string TimeSpent { get; set; }
        public string Handler { get; set; }
        public string Status { get; set; }

        public TaskDTO() { } //Пустой конструктор для входных данных от клиента

        public TaskDTO(TechTask task) //Конструктор для выходных данных клиенту
        {
            Id = task.Id;
            Description = task.Description;
            TimeSpent = task.TimeSpent.ToString(@"hh\:mm\:ss");
            Handler = task.Handler?.Name;
            Status = task.Status.ToString();
        }
    }
}