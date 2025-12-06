using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PlantCare.Models;
using PlantCare.Utils;

namespace PlantCare.Data
{
    public class FilePlantRepository : IPlantRepository
    {
        private readonly string _filePath;

        public FilePlantRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<Plant> Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<Plant>();

                var json = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<Plant>();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var plants = JsonSerializer.Deserialize<List<Plant>>(json, options);
                return plants ?? new List<Plant>();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return new List<Plant>();
            }
        }

        public void Save(List<Plant> plants)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(plants, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }
    }
}
