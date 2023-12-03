using eFitnessApp.Helpers;
using eFitnessApp.Models;
using eFitnessApp.Services.Abstruct;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eFitnessApp.Services.Implement
{
    public class MealService : IMealService
    {
        private readonly SQLiteConnection _database;



        // In the constructor we init the SQLite database
        public MealService()
        {
            _database = DatabaseContext.Database;
        }



        /// <summary>
        /// This is a task for insert an meal in the SQLite
        /// </summary>
        /// <param name="meal"></param>
        /// <returns></returns>
        public Task<bool> Create(Meal meal)
        {   
            _database.Insert(meal);
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for update an meal in the SQLite
        /// </summary>
        /// <param name="meal"></param>
        /// <returns></returns>
        public Task<bool> Update(Meal meal)
        {           
            _database.Update(meal);
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for delete all meals in the SQLite
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteAll()
        {
            _database.DeleteAll<Meal>();
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for delete single meal in the SQLite
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns></returns>
        public Task<bool> DeleteSingle(int mealId)
        {
            var meal = GetSingle(mealId).Result;
            _database.Delete(meal);
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for get all meals in the SQLite
        /// </summary>
        /// <returns></returns>
        public Task<List<Meal>> GetAll()
        {
            return Task.FromResult(_database.Table<Meal>().ToList());
        }



        /// <summary>
        /// This is a task for get single meal in the SQLite
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns></returns>
        public Task<Meal> GetSingle(int mealId)
        {
            return Task.FromResult(_database.Table<Meal>().First(x => x.Id == mealId));
        }
    }
}
