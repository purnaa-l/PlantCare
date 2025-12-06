using System;

namespace PlantCare.Models
{
    public class PlantTask
    {
        public Guid PlantId { get; set; }
        public string PlantName { get; set; } = string.Empty;
        public string TaskType { get; set; } = string.Empty; // Watering / Fertilizing
        public DateTime DueDate { get; set; }

        public override string ToString()
        {
            return $"{TaskType} - {PlantName} (Due: {DueDate:d})";
        }
    }
}
