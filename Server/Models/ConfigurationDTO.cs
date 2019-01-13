using Service;

namespace Server.Models
{
    public class ConfigurationDTO
    {
        public int RangeMin { get; set; }
        public int RangeMax { get; set; }
        public int Tm { get; set; }
        public int Td { get; set; }

        public ConfigurationDTO() { } //Пустой конструктор для входных данных от клиента

        public ConfigurationDTO(TechServiceConfig config)
        {
            RangeMin = (int)config.TimeRange.MinTime.TotalSeconds;
            RangeMax = (int)config.TimeRange.MaxTime.TotalSeconds;
            Tm = (int)config.Tm.TotalSeconds;
            Td = (int)config.Td.TotalSeconds;
        }
    }
}