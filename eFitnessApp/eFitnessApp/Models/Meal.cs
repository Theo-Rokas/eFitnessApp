using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace eFitnessApp.Models
{
    public class Meal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string NutritionType { get; set; }

        public int Frequency { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }
    }
}
