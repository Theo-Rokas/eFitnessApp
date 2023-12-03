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
    public class QuoteService : IQuoteService
    {
        private readonly SQLiteConnection _database;



        // In the constructor we init the SQLite database
        public QuoteService()
        {
            _database = DatabaseContext.Database;
        }



        /// <summary>
        /// This is a task for insert an quote in the SQLite
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public Task<bool> Create(Quote quote)
        {   
            _database.Insert(quote);
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for update an quote in the SQLite
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public Task<bool> Update(Quote quote)
        {           
            _database.Update(quote);
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for delete all quotes in the SQLite
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteAll()
        {
            _database.DeleteAll<Quote>();
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for delete single quote in the SQLite
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public Task<bool> DeleteSingle(int quoteId)
        {
            var quote = GetSingle(quoteId).Result;
            _database.Delete(quote);
            return Task.FromResult(true);
        }



        /// <summary>
        /// This is a task for get all quotes in the SQLite
        /// </summary>
        /// <returns></returns>
        public Task<List<Quote>> GetAll()
        {
            return Task.FromResult(_database.Table<Quote>().ToList());
        }



        /// <summary>
        /// This is a task for get single quote in the SQLite
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public Task<Quote> GetSingle(int quoteId)
        {
            return Task.FromResult(_database.Table<Quote>().First(x => x.Id == quoteId));
        }
    }
}
