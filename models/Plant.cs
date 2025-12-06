using System;
using System.Collections.Generic;

namespace PlantCare.Models
{
    public class Plant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public int WateringIntervalDays { get; set; }
        public int FertilizingIntervalDays { get; set; }

        public DateTime LastWatered { get; set; }
        public DateTime LastFertilized { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        public List<PlantLogEntry> Logs { get; set; } = new List<PlantLogEntry>();

        public override string ToString()
        {
            // This is what ListBox will show
            return Name;
        }
    }
}
