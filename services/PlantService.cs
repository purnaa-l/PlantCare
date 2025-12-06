using System;
using System.Collections.Generic;
using System.Linq;
using PlantCare.Data;
using PlantCare.Models;
using System.IO;
using System.Text;

namespace PlantCare.Services
{
    public class PlantService
    {
        private readonly IPlantRepository _repository;
        private readonly List<Plant> _plants;

        public PlantService(IPlantRepository repository)
        {
            _repository = repository;
            _plants = _repository.Load() ?? new List<Plant>();

            foreach (var p in _plants)
                p.Logs ??= new List<PlantLogEntry>();
        }

        public List<Plant> GetAllPlants() =>
            _plants.OrderBy(p => p.Name).ToList();

        public Plant AddPlant(Plant plant)
        {
            ValidatePlant(plant);
            plant.Id = Guid.NewGuid();
            plant.Logs ??= new List<PlantLogEntry>();
            _plants.Add(plant);
            _repository.Save(_plants);
            return plant;
        }

        public void UpdatePlant(Plant plant)
        {
            ValidatePlant(plant);
            var existing = _plants.FirstOrDefault(p => p.Id == plant.Id);
            if (existing == null)
                throw new ArgumentException("Plant not found.");

            existing.Name = plant.Name;
            existing.Species = plant.Species;
            existing.Location = plant.Location;
            existing.WateringIntervalDays = plant.WateringIntervalDays;
            existing.FertilizingIntervalDays = plant.FertilizingIntervalDays;
            existing.LastWatered = plant.LastWatered;
            existing.LastFertilized = plant.LastFertilized;
            existing.ImagePath = plant.ImagePath;
            existing.Logs = plant.Logs ?? new List<PlantLogEntry>();

            _repository.Save(_plants);
        }

        public void DeletePlant(Guid id)
        {
            var existing = _plants.FirstOrDefault(p => p.Id == id);
            if (existing != null)
            {
                _plants.Remove(existing);
                _repository.Save(_plants);
            }
        }

        public void AddLogEntry(Guid plantId, PlantLogEntry log)
        {
            var plant = _plants.FirstOrDefault(p => p.Id == plantId)
                ?? throw new ArgumentException("Plant not found.");

            plant.Logs ??= new List<PlantLogEntry>();
            plant.Logs.Add(log);
            _repository.Save(_plants);
        }

        public List<PlantTask> GetTasksForDate(DateTime date)
        {
            date = date.Date;
            var tasks = new List<PlantTask>();

            foreach (var plant in _plants)
            {
                if (plant.WateringIntervalDays > 0 && plant.LastWatered != DateTime.MinValue)
                {
                    var nextWater = plant.LastWatered.Date.AddDays(plant.WateringIntervalDays);
                    if (nextWater == date)
                    {
                        tasks.Add(new PlantTask
                        {
                            PlantId = plant.Id,
                            PlantName = plant.Name,
                            TaskType = "Watering",
                            DueDate = nextWater
                        });
                    }
                }

                if (plant.FertilizingIntervalDays > 0 && plant.LastFertilized != DateTime.MinValue)
                {
                    var nextFert = plant.LastFertilized.Date.AddDays(plant.FertilizingIntervalDays);
                    if (nextFert == date)
                    {
                        tasks.Add(new PlantTask
                        {
                            PlantId = plant.Id,
                            PlantName = plant.Name,
                            TaskType = "Fertilizing",
                            DueDate = nextFert
                        });
                    }
                }
            }

            return tasks
                .OrderBy(t => t.PlantName)
                .ThenBy(t => t.TaskType)
                .ToList();
        }

        private void ValidatePlant(Plant plant)
        {
            if (string.IsNullOrWhiteSpace(plant.Name))
                throw new ArgumentException("Name is required.");

            if (plant.WateringIntervalDays < 0 || plant.FertilizingIntervalDays < 0)
                throw new ArgumentException("Intervals must be >= 0.");
        }

public void ExportToCsv(string csvPath)
{
    var lines = new List<string>();

    // if file doesn't exist, write header once
    if (!File.Exists(csvPath))
    {
        lines.Add("ExportedAt,Id,Name,Species,Location,WaterIntervalDays,FertilizingIntervalDays,LastWatered,LastFertilized");
    }

    string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    foreach (var p in _plants)
    {
        string Escape(string v) => "\"" + (v ?? "").Replace("\"", "\"\"") + "\"";

        lines.Add(string.Join(",",
            "\"" + now + "\"",
            p.Id,
            Escape(p.Name),
            Escape(p.Species),
            Escape(p.Location),
            p.WateringIntervalDays,
            p.FertilizingIntervalDays,
            p.LastWatered.ToString("yyyy-MM-dd"),
            p.LastFertilized.ToString("yyyy-MM-dd")
        ));
    }

    // append to CSV; old records stay even if plants are later deleted
    File.AppendAllLines(csvPath, lines);
}
    }}