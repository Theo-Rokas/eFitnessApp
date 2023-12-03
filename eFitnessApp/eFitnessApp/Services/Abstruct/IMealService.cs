using eFitnessApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eFitnessApp.Services.Abstruct
{
    public interface IMealService
    {
        Task<bool> Create(Meal meal);

        Task<bool> Update(Meal meal);

        Task<bool> DeleteSingle(int mealId);

        Task<bool> DeleteAll();

        Task<Meal> GetSingle(int mealId);

        Task<List<Meal>> GetAll();
    }
}
