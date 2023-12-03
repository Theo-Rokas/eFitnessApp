using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace eFitnessApp.Models
{
    public class Exercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string MuscleType { get; set; }

        public int Reps { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }
    }
}
