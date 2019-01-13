using Service;

namespace Server.Models
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public bool IsBusy { get; set; }

        public EmployeeDTO() { } //Пустой конструктор для входных данных от клиента

        public EmployeeDTO(Employee employee) //Конструктор для выходных данных клиенту
        {
            Id = employee.Id;
            Name = employee.Name;
            IsBusy = employee.IsBusy;
            Position = employee.PositionName;
        }
    }
}