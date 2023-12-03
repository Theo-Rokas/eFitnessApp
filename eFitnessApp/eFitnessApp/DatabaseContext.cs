using eFitnessApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eFitnessApp
{
    public static class DatabaseContext
    {
        public static SQLiteConnection Database;

        public static void Initialize()
        {
            if (Database == null)
            {
                Database = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "eFitnessApp.db3"));
                Database.CreateTable<Exercise>();
                Database.CreateTable<Meal>();
                Database.CreateTable<Quote>();
            }
        }
    }
}
