using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace eFitnessApp.Models
{
    public class Quote
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Author { get; set; }

        [MaxLength(500)]
        public string Words { get; set; }
    }
}
