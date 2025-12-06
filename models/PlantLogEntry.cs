using System;

namespace PlantCare.Models
{
    public class PlantLogEntry
    {
        public DateTime Date { get; set; }
        public string Note { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Date:g} - {Note}";
        }
    }
}
