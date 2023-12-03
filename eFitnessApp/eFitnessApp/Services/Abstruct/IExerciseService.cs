using eFitnessApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eFitnessApp.Services.Abstruct
{
    public interface IExerciseService
    {
        Task<bool> Create(Exercise exercise);

        Task<bool> Update(Exercise exercise);

        Task<bool> DeleteSingle(int exerciseId);

        Task<bool> DeleteAll();

        Task<Exercise> GetSingle(int exerciseId);

        Task<List<Exercise>> GetAll();
    }
}
