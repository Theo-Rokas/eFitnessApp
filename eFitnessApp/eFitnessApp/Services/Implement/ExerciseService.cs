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
    public class ExerciseService : IExerciseService
    {
        private readonly SQLiteConnection _database;



        // In the constructor we init the SQLite database
        public ExerciseService()
        {
            _database = DatabaseContext.Database;
        }



        /// <summary>
        /// This is a task for insert an exercise in the SQLite
        /// </summary>
        /// <param name="exercise"></param>
        /// <returns></returns>
        public Task<bool> Create(Exercise exercise)
        {   
            _database.Insert(exercise);
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for update an exercise in the SQLite
        /// </summary>
        /// <param name="exercise"></param>
        /// <returns></returns>
        public Task<bool> Update(Exercise exercise)
        {           
            _database.Update(exercise);
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for delete all exercises in the SQLite
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteAll()
        {
            _database.DeleteAll<Exercise>();
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for delete single exercise in the SQLite
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        public Task<bool> DeleteSingle(int exerciseId)
        {
            var exercise = GetSingle(exerciseId).Result;
            _database.Delete(exercise);
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for get all exercises in the SQLite
        /// </summary>
        /// <returns></returns>
        public Task<List<Exercise>> GetAll()
        {
            return Task.FromResult(_database.Table<Exercise>().ToList());
        }



        /// <summary>
        /// This is a task for get single exercise in the SQLite
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        public Task<Exercise> GetSingle(int exerciseId)
        {
            return Task.FromResult(_database.Table<Exercise>().First(x => x.Id == exerciseId));
        }
    }
}
