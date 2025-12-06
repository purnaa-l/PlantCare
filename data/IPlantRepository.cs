using System.Collections.Generic;
using PlantCare.Models;

namespace PlantCare.Data
{
    public interface IPlantRepository
    {
        List<Plant> Load();
        void Save(List<Plant> plants);
    }
}
