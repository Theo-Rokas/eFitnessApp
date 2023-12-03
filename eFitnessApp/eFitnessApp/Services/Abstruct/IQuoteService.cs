using eFitnessApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eFitnessApp.Services.Abstruct
{
    public interface IQuoteService
    {
        Task<bool> Create(Quote quote);

        Task<bool> Update(Quote quote);

        Task<bool> DeleteSingle(int quoteId);

        Task<bool> DeleteAll();

        Task<Quote> GetSingle(int quoteId);

        Task<List<Quote>> GetAll();
    }
}
